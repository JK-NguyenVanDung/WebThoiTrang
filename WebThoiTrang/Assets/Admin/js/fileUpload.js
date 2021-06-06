const dropArea = document.querySelector(".drag-area"),
    icon = dropArea.querySelector("div"),
    dragText = dropArea.querySelector("header"),
    or = dropArea.querySelector("span"),
    button = dropArea.querySelector("#browse"),
    input = dropArea.querySelector("input"),
    note = dropArea.querySelector("p");
let file; //this is a global variable and we'll use it inside multiple functions

button.onclick = () => {
    input.click(); //if user click on the button then the input also clicked
}

input.addEventListener("change", function () {
    //getting user select file and [0] this means if user select multiple files then we'll select only the first one
    file = this.files[0];
    dropArea.classList.add("active");
    showFile(); //calling function
});


//If user Drag File Over DropArea
dropArea.addEventListener("dragover", (event) => {
    event.preventDefault(); //preventing from default behaviour
    dropArea.classList.add("active");
    dragText.textContent = "Release to Upload File";
});

//If user leave dragged File from DropArea
dropArea.addEventListener("dragleave", () => {
    dropArea.classList.remove("active");
    dragText.textContent = "Drag & Drop to Upload File";
});

//If user drop File on DropArea
dropArea.addEventListener("drop", (event) => {
    event.preventDefault(); //preventing from default behaviour
    //getting user select file and [0] this means if user select multiple files then we'll select only the first one
    file = event.dataTransfer.files[0];
    showFile(); //calling function
});

function showFile() {
    let fileType = file.type; //getting selected file type
    let validExtensions = ["image/jpeg", "image/jpg", "image/png"]; //adding some valid image extensions in array
    if (validExtensions.includes(fileType)) { //if user selected file is an image file
        let fileReader = new FileReader(); //creating new FileReader object
        fileReader.onload = () => {
            let fileURL = fileReader.result; //passing user file source in fileURL variable
            // UNCOMMENT THIS BELOW LINE. I GOT AN ERROR WHILE UPLOADING THIS POST SO I COMMENTED IT
            let imgTag = `<img src="${fileURL}" alt="image" id="uploaded-img">  <button style=" border: none;  position: absolute; top: 0; right:0; z-index: 100; color: black;" id="removeBtn" type="button" onclick="removeImg()"> Remove </button>`; //creating an img tag and passing user selected file source inside src attribute
            dropArea.innerHTML = imgTag; //adding that created img tag inside dropArea container
        }
        fileReader.readAsDataURL(file);
    } else {
        alert("This is not an Image File!");
        dropArea.classList.remove("active");
        dragText.textContent = "Drag & Drop to Upload File";
    }
}


function removeImg() {
    dropArea.classList.remove("active");

    let img = document.getElementById("uploaded-img");
    let removeButton = document.getElementById("removeBtn");

    dropArea.removeChild(img)
    dropArea.removeChild(removeButton)

    dropArea.appendChild(icon);
    dropArea.appendChild(dragText);
    dropArea.appendChild(or);
    dropArea.appendChild(button);
    dropArea.appendChild(input);
    dropArea.appendChild(note);

}
const img = document.querySelector(".uploaded-img"),
    delBtn = document.querySelector(".removeBtn");

function editImg() {
    alert("yeah");
    dropArea.removeChild(img)
    dropArea.removeChild(removeButton)
    dropArea.innerHTML(defaultStuff);
}

function replaceContent() {
    const image = dropArea.querySelector("img");
    const btn = dropArea.querySelector("button");
    console.log(dropArea);
    console.log(image);
    dropArea.classList.remove("active");

    console.log(btn);
    dropArea.removeChild(image);

    dropArea.removeChild(btn);
    dropArea.innerHTML = '<div class="icon" style="margin-top:10%;"><i class="fas fa-cloud-upload-alt"></i></div> <header style = "font-size: 1rem;" class="text-center " >Choose a file or drag it here.</header ><span>OR</span><button type="button" class="btn btn-primary " id="browse">Browse File</button><input type="file" hidden><p style="text-align:center; bottom: 0;">  (Accepted file type: PNG,JPEG,SVG)</p> ';

}