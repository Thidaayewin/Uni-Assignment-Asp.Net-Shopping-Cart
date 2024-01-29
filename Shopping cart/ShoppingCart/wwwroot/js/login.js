function onLogin(e) {
    e.preventDefault(); // prevent form from submitting

    let xhr = new XMLHttpRequest();
    xhr.open("POST", "/Account/onLogin");
    xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    xhr.onreadystatechange = function () {
        
        if (this.readyState == XMLHttpRequest.DONE) {
            let data = JSON.parse(this.responseText);
            if (data.status === 200) {
                // login successful, redirect to product list page
                window.location.href = "/ProductList";
            } else if (data.status === 401) {//not found
                swal("Account Not Exists!","user name is not exists!",  "error");
            } else if (data.status === 404) {//wrong password
                swal("Incorrect Password!","Please check your password!", "error");
            }
        }
    };

    let username = document.getElementById("username").value;
    let password = document.getElementById("password").value;
    xhr.send("username=" + encodeURIComponent(username) + "&password=" + encodeURIComponent(password));
}

let loginForm = document.getElementById("login-form");
loginForm.addEventListener("submit", onLogin);
