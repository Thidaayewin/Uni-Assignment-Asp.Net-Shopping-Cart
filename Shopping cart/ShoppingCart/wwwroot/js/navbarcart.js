/*fetch number of items in cart*/
addCount();

function addCount() {
    let xhr = new XMLHttpRequest();
    xhr.open("POST", "/Product/AddCount");
    xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    xhr.onreadystatechange = function () {
        if (this.readyState == XMLHttpRequest.DONE) {
            let data = JSON.parse(this.responseText);
            fixCount(data.total_quantity);
        }
    };
    xhr.send();
}

function fixCount(count) {
    var lblCartCount = document.getElementById("lblCartCount");
    lblCartCount.textContent = count;
}
