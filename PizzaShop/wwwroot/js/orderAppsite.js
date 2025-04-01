const navToggle = document.getElementById('navToggle');
const menudropdown = document.getElementById('navDropdown');

let flag1 = 0;
navToggle.addEventListener("click", () => {
    flag1 = !flag1;
    if (flag1) {
        menudropdown.style.display = "block";
    } else {
        menudropdown.style.display = "none";

    }
})
