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
            <h2>${post.title}</h2>
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






 window.onload = loadPosts;