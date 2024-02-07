const actualBtn = document.getElementById('actual-btn');
const fileChosen = document.getElementById('file-chosen');
actualBtn.addEventListener('change', function () {
    fileChosen.textContent = this.files[0].name
})

// window.onload=(){
//     var arr=document.cookie.split("=");

// } 

function dark() {
    var theme = document.querySelector('body').getAttribute('data-bs-theme');
    var maincontent = document.getElementById('maincontent');
    var navbar = document.getElementById('navbar');
    if (theme == 'light') {
        document.querySelector('body').setAttribute('data-bs-theme', 'dark');
        maincontent.classList.replace('bg-white', 'bg-dark');
        navbar.classList.replace('bg-white', 'bg-dark');
        document.cookie = "theme= " + theme;
    }
    else {
        document.querySelector('body').setAttribute('data-bs-theme', 'light');
        maincontent.classList.replace('bg-dark', 'bg-white');
        navbar.classList.replace('bg-dark', 'bg-white');
        document.cookie = "theme= " + theme;
    }
}

function passtoggle() {
    // console.log('hello');
    var x = document.getElementById('togpass').getAttribute('type');
    // console.log(x);
    if (x == "password") {
        document.getElementById('togpass').setAttribute('type', 'text');
        document.getElementById('passicon').classList.replace('fa-eye-slash', 'fa-eye');
    }
    else {
        document.getElementById('togpass').setAttribute('type', 'password');
        document.getElementById('passicon').classList.replace('fa-eye', 'fa-eye-slash');
    }

}
function country_code() {
    console.log('hello');
    var ele = document.getElementById('country').value;
    if (ele === "select country") {
        document.getElementById('output').value = "";
    }
    else if (ele === "us") {
        document.getElementById('output').value = "+1";
    }
    else if (ele === "uk") {
        document.getElementById('output').value = "+44";
    }
    else if (ele === "br") {
        document.getElementById('output').value = "+55";
    }
    else if (ele === "ind") {
        document.getElementById('output').value = "+91";
    }

}
function country_code2() {
    var ele = document.getElementById('country2').value;
    console.log(ele);
    if (ele === "select country") {
        document.getElementById('output2').value = "";
    }
    else if (ele === "us") {
        document.getElementById('output2').value = "+1";
    }
    else if (ele === "uk") {
        document.getElementById('output2').value = "+44";
    }
    else if (ele === "br") {
        document.getElementById('output2').value = "+55";
    }
    else if (ele === "ind") {
        document.getElementById('output2').value = "+91";
    }

}