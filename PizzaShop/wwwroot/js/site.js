

var x = window.matchMedia("(max-width: 990px)")
    let flag=1;
    const toggleButton = document.getElementById('toggle');
    const sidebar = document.getElementById("sidebar");
    const content = document.getElementById('content');
    const close = document.getElementById('close');
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
    })

if(close){
    close.addEventListener('click',()=>{
        flag-0;
        sidebar.style.display="none";
    })
}

    toggleButton.addEventListener("click", () => {
        flag=1;
        if (flag) {
            if (x.matches) {
                sidebar.style.display = "block";
                content.style.marginLeft = "50px";
            }else {
                sidebar.style.display = "block";
                content.style.marginLeft = "270px";
            }
        }else{
            if (x.matches) {
                sidebar.style.display = "none";
                content.style.marginLeft = "50px";
            }else {
                sidebar.style.display = "block";
                content.style.marginLeft = "270px";

            }
        }

    })
    
    
    //For navbar items toggle list
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
   
    let showPass = 0;
    const p = document.getElementById("pwd");
    document.getElementById("eye").addEventListener('click', () => {
        console.log("hi");
        showPass = !showPass
        if (showPass) {
            p.setAttribute('type', 'text');
        } else {
            p.setAttribute('type', 'password');
        }
    
    })

let showpass1 = 0;
let showpass2 = 0;
const cp = document.getElementById("confirmpwd");
const curp = document.getElementById("curpwd");

document.getElementById("confirmeye").addEventListener('click', () => {
showpass1 = !showpass1
if (showpass1) {
cp.setAttribute('type', 'text');
} else {
cp.setAttribute('type', 'password');
}
})
document.getElementById("cureye").addEventListener('click', () => {
showpass2 = !showpass2
if (showpass2) {
curp.setAttribute('type', 'text');
} else {
curp.setAttribute('type', 'password');
}

})
    

function search(){
    var input, filter, table, tr, td, i, txtValue;
    input = document.getElementById("search");
    filter = input.value.toUpperCase();
    table = document.getElementById("myTable");
    tr = table.getElementsByTagName("tr");
    console.log(input.value);
    for (i = 0; i < tr.length; i++) {
      td = tr[i].getElementsByTagName("td")[0];
      if (td) {
        txtValue = td.textContent || td.innerText;
        if (txtValue.toUpperCase().indexOf(filter) > -1) {
          tr[i].style.display = "";
        } else {
          tr[i].style.display = "none";
        }
      }       
    }
}

