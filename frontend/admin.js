
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
        const response = await fetch(`https://localhost:7109/wr/home/admin/postsWithComplaints`);
        
        if(!response.ok) throw new Error("Server error");
        
        const posts = await response.json();

        const postFeed = document.getElementById("postFeed");
    
        postFeed.innerHTML = "";

        posts.forEach( post => {
            
            const postBlok = document.createElement("div");

            postBlok.innerHTML = `
                <p>${post.content}</p>

                <button class = "likeButton" data-post-id="${post.postId}">🤍</button>

                <span>${post.reactionCount}</span>

                <button id="showCommentsButton" lang="uk">Переглянути коментарі</button>

                <hr>
            `;
            postFeed.appendChild(postBlok);

            //Show comments
            const showCommentsButton = postBlok.querySelector("#showCommentsButton");
            
            showCommentsButton.addEventListener("click", async () => {window.location.href = `comments.html?postId=${post.postId}`});
        });
    } catch(error) {console.log(error);}
}


window.onload = checkAuth;