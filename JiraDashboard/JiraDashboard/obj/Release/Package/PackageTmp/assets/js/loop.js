var repeat = document.getElementById("RepeatInterval").value * 60000;
var isLoop = false;

var t = window.setInterval(function () {
    if (isLoop) {
        document.getElementById("getresult").click();
    }
}, repeat);

function SetTimer() {
    document.getElementById("loop").setAttribute('onclick', 'CrushTimer()');
    document.getElementById("loop").setAttribute('title', 'End Loop');
    document.getElementById("loop").src = "/assets/gif/Reload.gif";
    isLoop = true;
}

function CrushTimer() {
    document.getElementById("loop").setAttribute('onclick', 'SetTimer()')
    document.getElementById("loop").setAttribute('title', 'Loop');
    document.getElementById("loop").src = "/assets/img/Reload.png";
    isLoop = false;
}       