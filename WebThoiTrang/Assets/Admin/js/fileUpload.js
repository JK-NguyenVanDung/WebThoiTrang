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
            dropArea.removeChild(icon);
            dropArea.removeChild(dragText);
            dropArea.removeChild(or);
            dropArea.removeChild(button);
            dropArea.removeChild(note);
            //dropArea.innerHTML += imgTag;

 //adding that created img tag inside dropArea container
        }
        fileReader.readAsDataURL(file);
    } else {
        alert("This is not an Image File!");
        dropArea.classList.remove("active");
        dragText.textContent = "Drag & Drop to Upload File";
    }
}


function removeImg() {
    location.reload();
    return false;

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
    dropArea.innerHTML = '<div class="icon" style="margin-top:10%;"><i class="fas fa-cloud-upload-alt"></i></div> <header style = "font-size: 1rem;" class="text-center " >Choose a file or drag it here.</header ><span>OR</span><button type="button" class="btn btn-primary " id="browse">Browse File</button><input type="file" multiple="multiple" hidden><p style="text-align:center; bottom: 0;">  (Accepted file type: PNG,JPEG,SVG)</p> ';

}