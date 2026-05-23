const sendButton = document.getElementById("sendPostButton");


async function SendComment()
{
    const formContent = document.getElementById("postFormInput").value;
    const userId = sessionStorage.getItem("userId");

    try
    {
        const response = await fetch(`https://localhost:7109/wr/home/${userId}`,{
            method: 'POST',
            headers: {'Content-type' : 'application/json'},
            body: JSON.stringify(formContent)
    });

    if(!response.ok) {throw new Error("Server error")}


    } catch(error) {
        console.error("Error", error);
        alert("Sending of post failed");
    }


} 


sendButton.addEventListener('click', SendComment);