
const userName = document.getElementById("userName");
const userNameFromStorage = sessionStorage.getItem("userName");

userName.textContent = userNameFromStorage;

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

            <button class = "likeButton" data-post-id="${post.postId}">
                ${post.isliked ? "❤️" : "🤍"}
            </button>

            <span>${post.reactionCount}</span>

            <hr>
        `;

            postFeed.appendChild(postBlok);

            const likeButton = postBlok.querySelector(".likeButton");

            likeButton.addEventListener("click", async () => {

                const postId = post.postId;

                try
                {
                    const response = await fetch(`https://localhost:7109/wr/home/setlike/${postId}/${userId}`, {
                        method: "POST"

                    });

                    if(!response.ok) {throw new Error("Like error");}

                    if(likeButton.textContent === "🤍") {likeButton.textContent = "❤️";}
                    else 
                    {
                        likeButton.textContent = "🤍";
                    }
                } catch(error) {console.log(error);}

                
            });
        });
    } catch(error)
    {   
        console.log(error);

        document.getElementById("postFeed").innerHTML = "The \"posts\" not found"
    }
}


window.onload = checkAuth;