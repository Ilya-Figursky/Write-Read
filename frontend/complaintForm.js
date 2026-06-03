const userId = sessionStorage.getItem("userId");

function getPostIdFromUrl() {
    const params = new URLSearchParams(window.location.search);
    return params.get('postId');
}

const postId = getPostIdFromUrl();

const but1 = document.getElementById("comp1");
const but2 = document.getElementById("comp2");
const but3 = document.getElementById("comp3");
const but4 = document.getElementById("comp4");



async function SendComplaintBut1() 
{
    const reason = but1.textContent;
    try
    {
        const response = await fetch(`https://localhost:7109/wr/home/complaint/${userId}/${postId}`,
        {
            method: 'POST',
            headers: {'Content-type' : 'application/json'},
            body: JSON.stringify(reason)
        })
        
         if(!response.ok) throw new Error("Server error");
    }
    catch(error){console.log(error);}
}

async function SendComplaintBut2() 
{
    const reason = but2.textContent;
    try
    {
        const response = await fetch(`https://localhost:7109/wr/home/complaint/${userId}/${postId}`,
        {
            method: 'POST',
            headers: {'Content-type' : 'application/json'},
            body: JSON.stringify(reason)
        })
        
         if(!response.ok) throw new Error("Server error");
    }
    catch(error){console.log(error);}
}

async function SendComplaintBut3() 
{
    const reason = but3.textContent;
    try
    {
        const response = await fetch(`https://localhost:7109/wr/home/complaint/${userId}/${postId}`,
        {
            method: 'POST',
            headers: {'Content-type' : 'application/json'},
            body: JSON.stringify(reason)
        })
        
         if(!response.ok) throw new Error("Server error");
    }
    catch(error){console.log(error);}
}

async function SendComplaintBut4() 
{
    const reason = but4.textContent;
    try
    {
        const response = await fetch(`https://localhost:7109/wr/home/complaint/${userId}/${postId}`,
        {
            method: 'POST',
            headers: {'Content-type' : 'application/json'},
            body: JSON.stringify(reason)
        })
        
         if(!response.ok) throw new Error("Server error");
    }
    catch(error){console.log(error);}
}





but1.addEventListener('click', SendComplaintBut1);
but2.addEventListener('click', SendComplaintBut2);
but3.addEventListener('click', SendComplaintBut3);
but4.addEventListener('click', SendComplaintBut4);