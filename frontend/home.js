
const userName = document.getElementById("userName");
const userNameFromStorage = sessionStorage.getItem("userName");

userName.textContent = userNameFromStorage;


function checkAuth()
{
    const userId = sessionStorage.getItem("userId");

    if(userId) 
    {
        loadPosts();
    }

    console.log("Authorization error");
}

async function loadPosts()
{
    try
    {
        const response = await fetch("https://localhost:7109/wr/home");
        
        if(!response.ok) throw new Error("Server error");
        
        const posts = await response.json();

        const postFeed = document.getElementById("postFeed");
    
        postFeed.innerHTML = "";

        posts.forEach( post => {
            
        const postBlok = document.createElement("div");

        postBlok.innerHTML = `
            <p>${post.content}</p>
            <hr>
        `;

            postFeed.appendChild(postBlok);
        });
    } catch(error)
    {   
        console.log(error);

        document.getElementById("postFeed").innerHTML = "The \"posts\" not found"
    }
}


window.onload = checkAuth;