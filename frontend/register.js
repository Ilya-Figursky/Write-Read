const signUpButton = document.getElementById("signUpButton");
const signInButton = document.getElementById("signInButton");

async function SignUp()
{
    const login = document.getElementById("registerLoginInput").value;
    const password = document.getElementById("registerPasswordInput").value;

    const userData = {
        login: login,
        password: password
    };

    try
    {
        const response = await fetch("https://localhost:7109/wr/register/signup", {
            method: 'POST', 
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(userData)

        });

        if(!response.ok) {throw new Error("Connection error or Sign up error")}

        const result = await response.json();

        console.log("Successfully");
        alert("Successfully sign up");
    } catch(error)
    {
        console.error("Error", error);
        alert("Sign up failed");
    }
}

async function SignIn()
{
    const login = document.getElementById("registerLoginInput").value;
    const password = document.getElementById("registerPasswordInput").value;

    const userData = {
        login: login,
        password: password
    };

    try
    {
        const response1 = await fetch("https://localhost:7109/wr/register/signin",{
            method: 'POST',
            headers: {'Content-type' : 'application/json'},
            body: JSON.stringify(userData)
        });

        if(!response1.ok) {throw new Error("Connection error or Sign in error")}

        const result = await response1.json();

        console.log("Successfully");
        alert("Successfully sign in");

        sessionStorage.setItem("userId", result.id)

        window.location.href = "home.html";
    } catch(error)
    {
        console.error("Error", error);
        alert("Sign up failed");
    }
    
}

signUpButton.addEventListener('click', SignUp);
signInButton.addEventListener('click', SignIn);
//window.location.href = "page.html";