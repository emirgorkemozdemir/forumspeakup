let mediaRecorder;
let audioChunks = [];
let audioStream;
const recognition = new webkitSpeechRecognition();
recognition.continuous = true;
recognition.interimResults = true;

recognition.onresult = function (event) {
    let interimTranscript = '';
    let finalTranscript = '';

    for (let i = event.resultIndex; i < event.results.length; ++i) {
        if (event.results[i].isFinal) {
            finalTranscript += event.results[i][0].transcript;
        } else {
            interimTranscript += event.results[i][0].transcript;
        }
    }

    document.getElementById('hiddenaudiotext').value = finalTranscript;
   
};

recognition.addEventListener('end', () => {
    convertToMP3();
});

function startRecording() {
    navigator.mediaDevices.getUserMedia({ audio: true })
        .then(function (stream) {
            audioStream = stream;
            mediaRecorder = new MediaRecorder(stream);
            mediaRecorder.start();
            recognition.start();
        })
        .catch(function (err) {
            console.error('Error accessing microphone: ' + err);
        });
}

function stopRecording() {
    recognition.stop();
    mediaRecorder.stop();
    audioStream.getTracks().forEach(track => track.stop());
    mediaRecorder.addEventListener('dataavailable', function (event) {
        if (event.data.size > 0) {
            audioChunks.push(event.data);
        }
    });
}

function convertToMP3() {
    const blob = new Blob(audioChunks, { type: 'audio/ogg; codecs=opus' });
    const reader = new FileReader();
    reader.readAsArrayBuffer(blob);
    reader.onloadend = function () {
        const arrayBuffer = reader.result;
        const audioBlob = new Blob([arrayBuffer], { type: 'audio/mp3' });
        const audioUrl = URL.createObjectURL(audioBlob);
        const audio = document.getElementById('audioPreview');
        audio.src = audioUrl;
        saveAudioFile(audioBlob);
    };
}

function saveAudioFile(blob) {
    const formData = new FormData();
    var user_id = document.getElementById('useridhidden').value;
    var audiotext = document.getElementById('hiddenaudiotext').value;
    debugger;
    var currentDatetime = Date.now();
    var voicename = (user_id + currentDatetime + ".mp3").toString();
    document.getElementById('audionamehidden').value = voicename;
    formData.append('audio', blob, voicename);
    formData.append('mytext', audiotext);

    debugger;
    fetch('/Main/UploadAudio', {
        method: 'POST',
        body: formData
    })
        .then(function (response) {
            if (response.ok) {
                return response.text();
            } else {
                throw new Error('Error saving audio!');
            }
        })
        .then(function (responseText) {
            const filePath = responseText;
            document.getElementById('hiddenVoiceLink').value =filePath.trim(); // Set the value of the input field
            console.log('Audio saved successfully! File path: ' + filePath);
        })
        .catch(function (err) {
            console.error('Error saving audio: ' + err);
        });
}