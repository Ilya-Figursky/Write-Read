
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

            //<p><i>${post.authorName}</i></p>
            postBlok.innerHTML = `
                
                <p>${post.content}</p>
                <p></p>

                <p lang="uk">Причина скарги: ${post.reason}</p>

                <button id="showCommentsButton" lang="uk">Переглянути коментарі</button>
                <button id="deletePostButton" lang="uk">Видалити</button>
                <button id="calncelComplaintButton" land="ud">Відхилити скаргу</button>

                

                <hr>
            `;
            postFeed.appendChild(postBlok);

            //Show comments
            const showCommentsButton = postBlok.querySelector("#showCommentsButton");
            
            showCommentsButton.addEventListener("click", async () => {window.location.href = `comments.html?postId=${post.postId}`});
        
            const deletePostButton = postBlok.querySelector("#deletePostButton");

            deletePostButton.addEventListener("click", async () => {

                const postId = post.postId;
                try
                {
                const response = await fetch(`https://localhost:7109/wr/home/deletePost/${postId}`,{method: "DELETE" });

                if(!response.ok) {throw new Error("Like error");}

                if(response.ok){location.reload(true);}

                }catch(error){console.log(error);}
            });
            
            const calncelComplaintButton = postBlok.querySelector("#calncelComplaintButton");
            calncelComplaintButton.addEventListener("click", async () => {

                const postId = post.postId;
                try
                {
                const response = await fetch(`https://localhost:7109/wr/home/admin/cancelComplaint/${postId}`,{method: "DELETE" });

                if(!response.ok) {throw new Error("Like error");}

                if(response.ok){location.reload(true);}

                }catch(error){console.log(error);}
            });
        
        });
    } catch(error) {console.log(error);}
}


window.onload = checkAuth;