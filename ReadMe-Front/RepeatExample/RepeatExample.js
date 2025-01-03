
fetch('/RepeatExample/NavBar.html')
        .then(response => response.text())
        .then(data => {
            document.getElementById('navbar').innerHTML = data;
        });

fetch('/RepeatExample/Progress.html')
        .then(response => response.text())
        .then(data => {
            document.getElementById('progress-placeholder').innerHTML = data;
        });