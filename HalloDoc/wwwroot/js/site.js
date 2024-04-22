﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var flag;

function showSessionExpiryPopup() {
    $.ajax({
        success: function (response) {
            Swal.fire({
                title: "Session Expire!",
                text: "Your Session is Expired",
                icon: "warning"
            }).then(function (result) {
                if (result.value) {
                    console.log("Yes Btn");
                    $.ajax({
                        success: function (response) {
                            window.location.href = "/Admin/Adminlogin";
                        },
                        error: function (xhr, status, error) {
                            alert("Error in _PartnerTable page");
                        }
                    })
                }
                else {
                    console.log('Cancelled!');
                }
            });
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
}
// Start a timer to show the popup before session expiration
function startSessionExpiryTimer() {
    const sessionTimeoutMinutes = 30; // Set this to match your session timeout
    const millisecondsBeforeExpiry = (sessionTimeoutMinutes - 1) * 60 * 1000;

    setTimeout(showSessionExpiryPopup, millisecondsBeforeExpiry);
}

// Call this function when the page loads
startSessionExpiryTimer();

var active = document.getElementsByClassName('active');
for (var i = 0; i < active.length; i++) {
    active[i].classList.add('border-bottom');
    active[i].classList.add('border-info');
}

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

function phydocupload(btn, file) {
    const actualBtn = document.getElementById(btn);
    const fileChosen = document.getElementById(file);
    var files = "";
    actualBtn.addEventListener('change', function () {
        for (var i = 0; i < this.files.length; i++) {
            files += this.files[i].name;
        }
        fileChosen.textContent = files;
    })
}


window.onload = function () {
    var array = document.cookie.split("=");

    flag = parseInt(array[1]);
    //console.log(localStorage.getItem('darkmode'));
    dark()
}

function dark() {
    try {
        //console.log(theme);
        var maincontent = document.getElementById('maincontent');
        var darkmode = document.getElementById('darkmode');
        var navlink = document.getElementsByClassName('nav-link');
        var active = document.getElementsByClassName('nav-link active');
        var activeState = document.getElementsByClassName('state active');
        var lightmode = document.getElementsByClassName('lightmode');
    } catch (error) { }
    if (localStorage.getItem('darkmode') == '0') {
        document.querySelector('body').setAttribute('data-bs-theme', 'dark');
        var thead = document.querySelector('thead');
        if (lightmode.length > 0) {
            for (var i = 0; i < lightmode.length; i++) {
                lightmode[i].classList.replace('bg-white', 'bg-dark');
            }
        }
        for (var i = 0; i < navlink.length; i++) {
            navlink[i].classList.add('text-white');
        }
        for (var i = 0; i < active.length; i++) {
            active[i].classList.remove('text-white');
            active[i].classList.add('text-info');
        }
        if (activeState.length > 0) {
            activeState[0].classList.remove('text-info');
        }
        $('.Patient').each(function () {
            $(this).css("--bs-table-bg", "rgb(2 77 0)");
        });
        $('.Family').each(function () {
            $(this).css("--bs-table-bg", "#693a00");
        });
        $('.Business').each(function () {
            $(this).css("--bs-table-bg", "#670033");
        });
        $('.Concierge').each(function () {
            $(this).css("--bs-table-bg", "#002345");
        });
        $('.New').css("background-color", "rgb(37 114 139)");
        $('.Pending').css("background-color", "rgb(42 107 147)");
        $('.Active').css("background-color", "rgb(15 81 15)");
        $('.Conclude').css("background-color", " rgb(93 15 27)");
        $('.Toclose').css("background-color", "rgb(21 24 97)");
        $('.Unpaid').css("background-color", "rgb(111 22 111)");

        $('.active.New').css("background-color", "#006789");
        $('.active.Pending').css("background-color", "#005081");
        $('.active.Active').css("background-color", "#004900");
        $('.active.Conclude').css("background-color", "#4b000b");
        $('.active.Toclose').css("background-color", "#000347");
        $('.active.Unpaid').css("background-color", "#6b006b");


        $('.active.state.New').addClass("dark");
        $('.active.state.Pending').addClass("dark");
        $('.active.state.Active').addClass("dark");
        $('.active.state.Conclude').addClass("dark");
        $('.active.state.Toclose').addClass("dark");
        $('.active.state.Unpaid').addClass("dark");


        $('.btn-info').each(function () {
            $(this).css("--bs-btn-bg", "#004857");
        });
        
        try {
            maincontent.classList.replace('bg-white', 'bg-dark');
            darkmode.classList.replace('bg-white', 'bg-dark');
        } catch (error) { }
    }
    else {
        document.querySelector('body').setAttribute('data-bs-theme', 'light');
        if (lightmode.length > 0) {
            for (var i = 0; i < lightmode.length; i++) {
                lightmode[i].classList.replace('bg-dark', 'bg-white');
            }
        }
        for (var i = 0; i < navlink.length; i++) {
            navlink[i].classList.remove('text-white');
        }
        for (var i = 0; i < active.length; i++) {
            active[i].classList.remove('text-info');
        }
        $('.Patient').each(function () {
            $(this).css("--bs-table-bg", "rgb(72 169 70)");
        });
        $('.Family').each(function () {
            $(this).css("--bs-table-bg", "#f5a33e");
        });
        $('.Business').each(function () {
            $(this).css("--bs-table-bg", "hotpink");
        });
        $('.Concierge').each(function () {
            $(this).css("--bs-table-bg", "dodgerblue");
        });
        $('.New').css("background-color", "lightblue");
        $('.Pending').css("background-color", "lightskyblue");
        $('.Active').css("background-color", "lightgreen");
        $('.Conclude').css("background-color", "lightpink");
        $('.Toclose').css("background-color", "cornflowerblue");
        $('.Unpaid').css("background-color", "violet");


        $('.active.New').css("background-color", "darkblue");
        $('.active.Pending').css("background-color", "deepskyblue");
        $('.active.Active').css("background-color", "darkgreen");
        $('.active.Conclude').css("background-color", "deeppink");
        $('.active.Toclose').css("background-color", "blue");
        $('.active.Unpaid').css("background-color", "darkviolet");

        $('.active.state.New').removeClass("dark");
        $('.active.state.Pending').removeClass("dark");
        $('.active.state.Active').removeClass("dark");
        $('.active.state.Conclude').removeClass("dark");
        $('.active.state.Toclose').removeClass("dark");
        $('.active.state.Unpaid').removeClass("dark");

        $('.btn-info').each(function () {
            $(this).css("--bs-btn-bg", "#0dcaf0");
        });
        try {
            maincontent.classList.replace('bg-dark', 'bg-white');
            darkmode.classList.replace('bg-dark', 'bg-white');
        } catch (error) { }
    }
}

function darktoggle() {
    if (localStorage.getItem('darkmode') == '0') {
        localStorage.setItem('darkmode', '1');
    }
    else {
        localStorage.setItem('darkmode', '0');
    }
    dark();
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
function SendAgreement(clicked_name, Requesttypeid, phonenumber, email) {
    var color = "mediumpurple";
    var type = "";
    if (Requesttypeid == 1) {
        color = "rgb(72 169 70)";
        type = "Patient";
    }
    else if (Requesttypeid == 2) {
        color = "#f5a33e";
        type = "Family";
    }
    else if (Requesttypeid == 4) {
        color = "hotpink";
        type = "Business";
    }
    else if (Requesttypeid == 3) {
        color = "dodgerblue";
        type = "Concierge";
    }
    document.getElementById('reqidSendAgreement').value = clicked_name;
    document.getElementById('coloragreement').style.backgroundColor = color;
    document.getElementById('typeagreement').innerHTML = type;
    document.getElementById('PhoneSendAgreement').value = phonenumber;
    document.getElementById('EmailSendAgreement').value = email;
}

function ChangePage(Page, status, reqtype, region, searchkey) {
    var currentPage = Page;
    var status = status;
    var regionid = region;
    var reqtypeid = reqtype;
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
        data: { "currentPage": currentPage, "regionid": regionid, "reqtypeid": reqtypeid, "status": status, "searchkey": searchkey },
        type: "POST",
        dataType: "html",
        success: function (response) {
            $('#status-tabContent').html(response);
            document.getElementById("page-1").style.backgroundColor = "white";
            document.getElementById("page-" + currentPage).style.backgroundColor = "lightblue";
            $('#exportpage').val(currentPage);
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });

}
function ContactBtn(phyid) {
    document.getElementById('ContactphyModal').innerHTML = phyid;
}

function ChangeShift(currentPage) {
    $.ajax({
        url: "/Admin/ShiftReviewTable",
        data: { "currentPage": currentPage, "regionid": $('#forregionid').val() },
        type: "POST",
        dataType: "html",
        success: function (response) {
            $('.shifttable').html(response);
            document.getElementById("page-" + currentPage).style.backgroundColor = "lightblue";
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });

}
function ChangeRecord(currentPage) {
    var form = $('#recordsForm');
    $('#curpageinput').val(currentPage);
    $.ajax({
        url: "/Admin/RecordsTable",
        data: form.serialize(),
        type: "POST",
        dataType: "html",
        success: function (response) {
            $('.recordstable').html(response);
            document.getElementById("page-" + currentPage).style.backgroundColor = "lightblue";
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });

}
function ChangeBlockHistory(currentPage) {
    var form = $('#BlockForm');
    $('#curpageinput').val(currentPage);
    $.ajax({
        url: "/Admin/BlockHistoryTable",
        data: form.serialize(),
        type: "POST",
        dataType: "html",
        success: function (response) {
            $('.blockhistorytable').html(response);
            document.getElementById("page-" + currentPage).style.backgroundColor = "lightblue";
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });

}
function ChangeEmaillogs(currentPage) {
    var form = $('#emaillogForm');
    $('#curpageinput').val(currentPage);
    $.ajax({
        url: "/Admin/EmailLogTable",
        data: form.serialize(),
        type: "POST",
        dataType: "html",
        success: function (response) {
            $('.emaillogtable').html(response);
            document.getElementById("page-" + currentPage).style.backgroundColor = "lightblue";
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });

}
function ChangeSearchRecord(currentPage) {
    var form = $('#SearchrecordsForm');
    $('#curpageinput').val(currentPage);
    $.ajax({
        url: "/Admin/SearchRecordTable",
        data: form.serialize(),
        type: "POST",
        dataType: "html",
        success: function (response) {
            $('.searchrecordstable').html(response);
            document.getElementById("page-" + currentPage).style.backgroundColor = "lightblue";
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });

}

function ChangePagePhy(Page, status, reqtype, region, searchkey) {
    var currentPage = Page;
    var status = status;
    var regionid = region;
    var reqtypeid = reqtype;
    var url;
    if (status == 1) {
        url = "/Physician/NewStatePhy/"
    }
    else if (status == 2) {
        url = "/Physician/PendingStatePhy/"
    }
    else if (status == 4) {
        url = "/Physician/ActiveStatePhy/"
    }
    else if (status == 6) {
        url = "/Physician/ConcludeStatePhy/"
    }
    else if (status == 3) {
        url = "/Physician/TocloseStatePhy/"
    }
    else {
        url = "/Physician/UnpaidStatePhy/"
    }
    $.ajax({
        url: url,
        data: { "currentPage": currentPage, "regionid": regionid, "reqtypeid": reqtypeid, "status": status, "searchkey": searchkey },
        type: "POST",
        dataType: "html",
        success: function (response) {
            $('#status-tabContent').html(response);
            document.getElementById("page-1").style.backgroundColor = "white";
            document.getElementById("page-" + currentPage).style.backgroundColor = "lightblue";
            $('#exportpage').val(currentPage);
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });

}

function EncounterModalOpen(reqid) {
    document.getElementById('reqidhidden').value = reqid;
}