
const userName = document.getElementById("userName");
const userNameFromStorage = sessionStorage.getItem("userName");

userName.textContent = userNameFromStorage;//set to show user's name on page

const userId = sessionStorage.getItem("userId");

function checkAuth()
{

    if(userId) 
    {
        loadPostsByUserId();
    } else
    {
        console.log("Authorization error");
    }
}

async function loadPostsByUserId()
{
    try
    {
        const response = await fetch(`https://localhost:7109/wr/home/${userId}`);
        
        if(!response.ok) throw new Error("Server error");
        
        const posts = await response.json();

        const postFeed = document.getElementById("postFeed");
    
        postFeed.innerHTML = "";

        posts.forEach( post => {
            
            const postBlok = document.createElement("div");

            postBlok.innerHTML = `
                <p>${post.content}</p>

                <button class = "likeButton" data-post-id="${post.postId}">${post.isLiked ? "❤️" : "🤍"}</button>

                <span>${post.reactionCount}</span>
 
                <button class = "complaintButton">
                    ${post.isComplaint ? "!" : "[!]"}
                </button>

                <button id="writeCommentButton" lang="uk">Написати коментар</button>
                <button id="showCommentsButton" lang="uk">Переглянути коментарі</button>

                <hr>
            `;

            postFeed.appendChild(postBlok);

            const likeButton = postBlok.querySelector(".likeButton");


            // like logic
            likeButton.addEventListener("click", async () => {  

                const postId = post.postId;

                //set like
                if(likeButton.textContent === "🤍") 
                {
                    try
                    {
                        const response = await fetch(`https://localhost:7109/wr/home/setlike/${postId}/${userId}`, {
                         method: "POST"
                        });

                     if(!response.ok) {throw new Error("Like error");}

                     if(likeButton.textContent === "🤍") {likeButton.textContent = "❤️";}
                     
                     
                    } catch(error) {console.log(error);}
                } else if(likeButton.textContent === "❤️")// remove like
                  {
                    try
                    {
                        const response = await fetch(`https://localhost:7109/wr/home/remuvelike/${postId}/${userId}`, {
                            method: "POST"
                        });
                            
                    
                        if(!response.ok) {throw new Error("Like error");}
                        if(likeButton.textContent === "❤️") {likeButton.textContent = "🤍";}
                    } catch(error){console.log(error);}

                     
                  } 
                
            });

            //Add comment
            const writeCommentButton = postBlok.querySelector("#writeCommentButton");

            writeCommentButton.addEventListener("click", async () => {window.location.href = `commentForm.html?postId=${post.postId}`});
            
            const showCommentsButton = postBlok.querySelector("#showCommentsButton");
            
            showCommentsButton.addEventListener("click", async () => {window.location.href = `comments.html?postId=${post.postId}`});
       
        });
    } catch(error) {console.log(error);}
}


window.onload = checkAuth;