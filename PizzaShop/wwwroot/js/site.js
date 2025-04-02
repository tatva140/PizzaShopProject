var x = window.matchMedia("(max-width: 990px)");
flag = 1;
if (document.getElementById("sidebar")) {
  toggleButton = document.getElementById("toggle");
  sidebar = document.getElementById("sidebar");
  content = document.getElementById("content");
  closebtn = document.getElementById("close");
  if (x.matches) {
    sidebar.style.display = "none";
    content.style.marginLeft = "50px";
  } else {
    sidebar.style.display = "block";
    content.style.marginLeft = "270px";
  }
  x.addEventListener("change", () => {
    if (x.matches) {
      sidebar.style.display = "none";
      content.style.marginLeft = "50px";
    } else {
      sidebar.style.display = "block";
      content.style.marginLeft = "270px";
    }
  });

  if (closebtn) {
    closebtn.addEventListener("click", () => {
      flag - 0;
      sidebar.style.display = "none";
    });
  }

  toggleButton.addEventListener("click", () => {
    flag = 1;
    if (flag) {
      if (x.matches) {
        sidebar.style.display = "block";
        content.style.marginLeft = "50px";
      } else {
        sidebar.style.display = "block";
        content.style.marginLeft = "270px";
      }
    } else {
      if (x.matches) {
        sidebar.style.display = "none";
        content.style.marginLeft = "50px";
      } else {
        sidebar.style.display = "block";
        content.style.marginLeft = "270px";
      }
    }
  });
}

//For navbar items toggle list
navToggle = document.getElementById("navToggle");
menudropdown = document.getElementById("navDropdown");

flag1 = 0;
navToggle.addEventListener("click", () => {
  flag1 = !flag1;
  if (flag1) {
    menudropdown.style.display = "block";
  } else {
    menudropdown.style.display = "none";
  }
});

// showPass = 0;
// p = document.getElementById("pwd");
// document.getElementById("eye").addEventListener("click", () => {
//   showPass = !showPass;
//   if (showPass) {
//     p.setAttribute("type", "text");
//   } else {
//     p.setAttribute("type", "password");
//   }
// });

// showpass1 = 0;
// showpass2 = 0;
// cp = document.getElementById("confirmpwd");
// curp = document.getElementById("curpwd");

// document.getElementById("confirmeye").addEventListener("click", () => {
//   showpass1 = !showpass1;
//   if (showpass1) {
//     cp.setAttribute("type", "text");
//   } else {
//     cp.setAttribute("type", "password");
//   }
// });
// document.getElementById("cureye").addEventListener("click", () => {
//   showpass2 = !showpass2;
//   if (showpass2) {
//     curp.setAttribute("type", "text");
//   } else {
//     curp.setAttribute("type", "password");
//   }
// });

function previewImage(e) {
  file = e.files[0];
  if (file) {
    validImageTypes = [
      "image/jpeg",
      "image/png",
      "image/gif",
      "image/webp",
      "images/jfif",
    ];
    if (!validImageTypes.includes(file.type)) {
      document.getElementById("img").style.display = "none";
      toastr.error("Invalid File Type! Please upload image.");
      e.value = "";
    } else {
      document.getElementById("img").style.display = "block";
      document.getElementById("img").src = URL.createObjectURL(file);
    }
  }
}
