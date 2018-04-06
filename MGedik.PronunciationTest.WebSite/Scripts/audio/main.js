/* Copyright 2013 Chris Wilson

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

window.AudioContext = window.AudioContext || window.webkitAudioContext;

var audioContext = new AudioContext();
var audioInput = null,
    realAudioInput = null,
    inputPoint = null,
    audioRecorder = null;
var recIndex = 0;

$(function () {
    $('#send').on("click", function () {
        audioRecorder.exportMonoWAV(function (blob) {
            var formData = new FormData();
            formData.append('file', blob);
            makeXMLHttpRequest('/sound/PostFormData', formData, function (response) {
                if (response.Status == "OK") {
                    $(".image-wrap").addClass("success");
                    setTimeout(function () {
                        location.reload();
                    }, 1500);
                } else {
                    $(".image-wrap").addClass("fail");
                }
            });
        });
    });

    $("#question").on("click", function () {
        $(".text").stop().fadeToggle();
    });

    $("#record").on("click",
        function (e) {
            toggleRecording(e.target);
        });
});

function makeXMLHttpRequest(url, data, callback) {
    var request = new XMLHttpRequest();
    request.onreadystatechange = function () {
        if (request.readyState == 4 && request.status == 200) {
            callback(JSON.parse(request.response));
        }
    };
    request.open('POST', url);
    request.send(data);
}

//function saveAudio() {
//    audioRecorder.exportWAV(doneEncoding);
//    // could get mono instead by saying
//    // audioRecorder.exportMonoWAV( doneEncoding );
//}

function gotBuffers(buffers) {
    // the ONLY time gotBuffers is called is right after a new recording is completed - 
    // so here's where we should set up the download.
    audioRecorder.exportWAV(doneEncoding);
}

function doneEncoding(blob) {
    Recorder.setupDownload(blob, "myRecording" + ((recIndex < 10) ? "0" : "") + recIndex + ".wav");
    recIndex++;
}

function toggleRecording(e) {
    if ($(e).hasClass("btn-danger")) {
        // stop recording
        audioRecorder.stop();
        $(e).removeClass("btn-danger");
        //audioRecorder.getBuffers(gotBuffers);
        $("#send").removeAttr("disabled");
    } else {
        // start recording
        if (!audioRecorder)
            return;
        $(e).addClass("btn-danger");
        audioRecorder.clear();
        audioRecorder.record();
        $("#send").attr("disabled", true);
        $(".image-wrap").removeClass("fail");
    }
}

function convertToMono(input) {
    var splitter = audioContext.createChannelSplitter(2);
    var merger = audioContext.createChannelMerger(2);

    input.connect(splitter);
    splitter.connect(merger, 0, 0);
    splitter.connect(merger, 0, 1);
    return merger;
}

function toggleMono() {
    if (audioInput != realAudioInput) {
        audioInput.disconnect();
        realAudioInput.disconnect();
        audioInput = realAudioInput;
    } else {
        realAudioInput.disconnect();
        audioInput = convertToMono(realAudioInput);
    }

    audioInput.connect(inputPoint);
}

function gotStream(stream) {
    inputPoint = audioContext.createGain();

    // Create an AudioNode from the stream.
    realAudioInput = audioContext.createMediaStreamSource(stream);
    audioInput = realAudioInput;
    audioInput.connect(inputPoint);

    //audioInput = convertToMono( input );

    audioRecorder = new Recorder(inputPoint);

    //var zeroGain = audioContext.createGain();
    ////zeroGain.gain.value = 0.0;
    //zeroGain.setTargetAtTime = 0.0;
    //inputPoint.connect(zeroGain);
    //zeroGain.connect(audioContext.destination);
}

function initAudio() {
    if (!navigator.getUserMedia)
        navigator.getUserMedia = navigator.webkitGetUserMedia || navigator.mozGetUserMedia;
    if (!navigator.cancelAnimationFrame)
        navigator.cancelAnimationFrame = navigator.webkitCancelAnimationFrame || navigator.mozCancelAnimationFrame;
    if (!navigator.requestAnimationFrame)
        navigator.requestAnimationFrame = navigator.webkitRequestAnimationFrame || navigator.mozRequestAnimationFrame;

    navigator.getUserMedia(
        {
            "audio": {
                "mandatory": {
                    "googEchoCancellation": "true",
                    "googAutoGainControl": "true",
                    "googNoiseSuppression": "true",
                    "googHighpassFilter": "false"
                },
                "optional": []
            },
        }, gotStream, function (e) {
            alert('Error getting audio');
            console.log(e);
        });
}

window.addEventListener('load', initAudio);
