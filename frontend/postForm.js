const sendButton = document.getElementById("sendPostButton");


async function SendComment()
{
    const inputElement = document.getElementById("postFormInput");
    const formContent = inputElement.value;
    const userId = sessionStorage.getItem("userId");

    if(formContent == "")
    {
        alert("Field is empty");
        return;
    }

    try
    {
        const response = await fetch(`https://localhost:7109/wr/home/${userId}`,{
            method: 'POST',
            headers: {'Content-type' : 'application/json'},
            body: JSON.stringify(formContent)
    });

    if(!response.ok) {throw new Error("Server error")}

    if(response.ok)
    {
        alert("Post succesfully sent");
        inputElement.value = "";
    }

    } catch(error) {
        console.error("Error", error);
        alert("Sending of post failed");
    }


} 


sendButton.addEventListener('click', SendComment);