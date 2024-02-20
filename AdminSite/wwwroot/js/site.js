var flag;
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