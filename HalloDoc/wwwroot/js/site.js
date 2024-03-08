// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var flag;
try {

    const actualBtn = document.getElementById('actual-btn');
    const fileChosen = document.getElementById('file-chosen');
    var files = "";
    actualBtn.addEventListener('change', function () {
        for (var i = 0; i < this.files.length; i++) {
            files += this.files[i].name;
        }
        fileChosen.textContent = files;
    })
}
catch (error) { }
console.log(document.cookie);
window.onload = function () {
    var array = document.cookie.split("=");
    console.log(array);
    flag = parseInt(array[1]);
    //console.log(flag);
    dark()
}

function dark() {
    try {
        //console.log(theme);
        var maincontent = document.getElementById('maincontent');
        var navbar = document.getElementById('navbar');
    } catch (error) { }
    if (flag == 0) {
        document.querySelector('body').setAttribute('data-bs-theme', 'dark');
        try {
            maincontent.classList.replace('bg-white', 'bg-dark');
            navbar.classList.replace('bg-white', 'bg-dark');
        } catch (error) { }
        document.cookie = "flag = " + flag;
        flag = 1;
    }
    else {
        document.querySelector('body').setAttribute('data-bs-theme', 'light');
        try {
            maincontent.classList.replace('bg-dark', 'bg-white');
            navbar.classList.replace('bg-dark', 'bg-white');
        } catch (error) { }
        document.cookie = "flag = " + flag;
        flag = 0;
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

function CancelModelName(clicked_name) {
    var arr = clicked_name.split("+");
    document.getElementById('cancleModelName').innerHTML = arr[0];
    document.getElementById('reqidCancelModel').innerHTML = arr[1];
}
function AssignModelName(clicked_name) {
    document.getElementById('reqidAssignModel').innerHTML = clicked_name;
}
function ClearCaseName(clicked_name) {
    document.getElementById('reqidClearCase').innerHTML = clicked_name;
}
function BlockModelName(clicked_name) {
    var arr = clicked_name.split("+");
    document.getElementById('BlockModelName').innerHTML = arr[0];
    document.getElementById('reqidBlockModel').innerHTML = arr[1];
}
function ChangePage(Page,status) {
    var currentPage = Page;
    var status = status;
    var url;
    if (status == 1) {
        url = "/Admin/NewState/"
    }
    else if (status == 2) {
        url = "/Admin/PendingState/"
    }
    else if (status == 4) {
        url = "/Admin/ActiveState/"
    }
    else if (status == 6) {
        url = "/Admin/ConcludeState/"
    }
    else if (status == 3) {
        url = "/Admin/TocloseState/"
    }
    else {
        url = "/Admin/UnpaidState/"
    }
    $.ajax({
        url: url,
        data: { "currentPage": currentPage },
        type: "POST",
        dataType: "html",
        success: function (response) {
            $('#status-tabContent').html(response);
            document.getElementById("page-1").style.backgroundColor = "white";
            document.getElementById("page-" + currentPage).style.backgroundColor = "lightblue";
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
}