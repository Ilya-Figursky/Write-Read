const userId = sessionStorage.getItem("userId");

const urlParams = new URLSearchParams(window.location.search);

const postId = urlParams.get('postId');

function checkAuth()
{

    if(userId) 
    {
        loadCommentsByUserId();
    } else
    {
        console.log("Authorization error");
    }
}

async function loadCommentsByUserId()
{
    try
    {
        const response = await fetch(`https://localhost:7109/wr/home/comments/getCommentsByPostIdUserId/${postId}/${userId}`);

        if(!response.ok) throw new Error("Server error");

        const comments = await response.json();

        const commentFeed = document.getElementById("commentFeed");
        
        commentFeed.innerHTML = "";

        comments.forEach( comment => {

            const commentBlok = document.createElement("div");

            commentBlok.innerHTML = `

            <p>${comment.content}</p>
            
            <button class ="likeButton" data-comment-id="${comment.commentId}">${comment.isLiked ? "❤️" : "🤍"}</button>

            <span>${comment.reactionCount}</span>

            <hr>
            `;


            commentFeed.appendChild(commentBlok);

            const likeButton = commentBlok.querySelector(".likeButton");

            // like logic
            likeButton.addEventListener("click", async () => {

                const commentId = comment.commentId;

                //set like
                if(likeButton.textContent === "🤍")
                {
                    try
                    {
                        const respontse = await fetch(`https://localhost:7109/wr/home/comments/setCommentLike/${userId}/${commentId}`, {
                            method: "POST"
                        });

                        if(!response.ok) {throw new Error("Like error");}

                        if(likeButton.textContent === "🤍") {likeButton.textContent = "❤️";}
                        

                    } catch(error){console.log(error);}
                } else if(likeButton.textContent === "❤️") // remove like
                {
                    try
                    {
                        const response = await fetch(`https://localhost:7109/wr/home/comments/deleteCommentLike/${userId}/${commentId}`, {
                            method: "DELETE"
                        });

                        if(!response.ok) {throw new Error("Like error");}

                        if(likeButton.textContent === "❤️") {likeButton.textContent = "🤍";}

                    } catch(error){console.log(error);}
                }
            });

        })
    } catch(error) {console.log(error);}
}

window.onload = checkAuth;