const signUpButton = document.getElementById("signUpButton");

async function SignUp()
{
    const login = document.getElementById("registerLoginInput").value;
    const password = document.getElementById("registerLoPasswordInput").value;

    const userData = {
        login: login,
        password: password
    };

    try
    {
        const response = await fetch("https://localhost:7109/wr/register", {
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

signUpButton.addEventListener('click', SignUp);