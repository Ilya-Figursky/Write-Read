const sendButton = document.getElementById("sendCommentButton");

function getPostIdFromUrl() {
    const params = new URLSearchParams(window.location.search);
    return params.get('postId');
}

async function SendComment()
{
    const formContent = document.getElementById("commentFormInput").value;
    const userId = sessionStorage.getItem("userId");
    const postId = getPostIdFromUrl(); // Получаем ID из URL
    
    if (!postId) {
        alert("Ошибка: не удалось определить ID поста");
        return;
    }

    try
    {
        const response = await fetch(`https://localhost:7109/wr/home/comments/saveComment/${userId}/${postId}`,{
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