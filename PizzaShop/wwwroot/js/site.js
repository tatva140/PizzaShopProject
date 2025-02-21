

var x = window.matchMedia("(max-width: 990px)")
    console.log(x);
    let flag=1;
    const toggleButton = document.getElementById('toggle');
    const sidebar = document.getElementById("sidebar");
    const content = document.getElementById('content');
    console.log(content);
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
        showPass = !showPass
        if (showPass) {
            p.setAttribute('type', 'text');
        } else {
            p.setAttribute('type', 'password');
        }
    
    })

    
// function save() {

//     const email = document.getElementById("email");
//     email.addEventListener('keypress', () => {
//         if (email.value.length != 0) {
//             emailnewDiv.innerHTML = "";

//         } else {
//             emailnewDiv.innerHTML = "Please enter a valid email";

//         }
//     })

//     const emailnewDiv = document.getElementById("emailerror");
//     if (email.value == '') {
//         emailnewDiv.innerHTML = "Please enter a valid email";
//     } else {
//         emailnewDiv.innerHTML = "";
//     }
// }




// function sort(n){
//     var table, rows, flag, i, x, y, swap, dir, swapcnt = 0;
//     table = document.getElementById("myTable");
//     flag = true;
//     // Set the sorting direction to ascending:
//     dir = "asc";
//     /* Make a loop that will continue until
//     no switching has been done: */
//     while (flag) {
//       // Start by saying: no switching is done:
//       flag = false;
//       rows = table.rows;
//       /* Loop through all table rows (except the
//       first, which contains table headers): */
//       for (i = 1; i < (rows.length - 1); i++) {
//         // Start by saying there should be no switching:
//         swap = false;
//         /* Get the two elements you want to compare,
//         one from current row and one from the next: */
//         x = rows[i].getElementsByTagName("TD")[n];
//         y = rows[i + 1].getElementsByTagName("TD")[n];
//         /* Check if the two rows should switch place,
//         based on the direction, asc or desc: */
//         if (dir == "asc") {
//           if (x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase()) {
//             // If so, mark as a switch and break the loop:
//             swap = true;
//             break;
//           }
//         } else if (dir == "desc") {
//           if (x.innerHTML.toLowerCase() < y.innerHTML.toLowerCase()) {
//             // If so, mark as a switch and break the loop:
//             swap = true;
//             break;
//           }
//         }
//       }
//       if (swap) {
//         /* If a switch has been marked, make the switch
//         and mark that a switch has been done: */
//         rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
//         flag = true;
//         // Each time a switch is done, increase this count by 1:
//         swapcnt ++;
//       } else {
//         /* If no switching has been done AND the direction is "asc",
//         set the direction to "desc" and run the while loop again. */
//         if (swapcnt == 0 && dir == "asc") {
//           dir = "desc";
//           flag = true;
//         }
//       }
//     }
//   }

function search(){
    var input, filter, table, tr, td, i, txtValue;
    input = document.getElementById("search");
    filter = input.value.toUpperCase();
    table = document.getElementById("myTable");
    tr = table.getElementsByTagName("tr");
    console.log(filter)
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


// const filename = document.getElementById('filename');
// const fileinfo = document.getElementById('fileinfo');

// filename.addEventListener('change', (e) => {
    
//     fileinfo.innerHTML = "Selected File: " + e.target.files[0].name;
// })

// function deleteUser(){
//     const modal=document.getElementById('myModal');
//     modal.style.display="flex";
// }
// function closeModal(n){
//     const modal=document.getElementById('myModal');
//     if(n==1){

//         modal.style.display="none";
//     }else{
//         modal.style.display="none";

//     }
// }