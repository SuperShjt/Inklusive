function deleteUser(userId) {
    if (confirm('Are you sure you want to delete this user?')) {
        fetch(`/User/DeleteUser?id=${userId}`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${localStorage.getItem('jwt')}`,  
            }
        })
            .then(response => {
                if (response.ok) {
                    alert('User deleted successfully.');
                    window.location.reload();  
                } else {
                    alert('Failed to delete user. / You are not authorized');
                }
            })
            .catch(error => {
                console.error('Error deleting user:', error);
                alert('An error occurred while trying to delete the user.');
            });
    }
}
window.onload = function () {
    const form = document.getElementById("updateProfileForm");
    if (!form) {
        console.error("Form not found! Check if the ID is correct.");
        return;
    }

window.onload = function () {
    const updateProfileForm = document.getElementById("updateProfileForm");
    if (updateProfileForm) {
        updateProfileForm.addEventListener("submit", async function (event) {
        event.preventDefault(); 

        const newUsername = document.getElementById("newUsername").value;
        console.log("Sending username: ", newUsername);
        const response = await fetch('/User/UpdateEProfile', {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                NewUsername: newUsername
            })
        });
        console.log("Sending JSON:", JSON.stringify({ NewUsername: newUsername }));

        if (response.ok) {
            alert('Profile update requested! It will be visible once approved by an admin.');
            window.location.href = "/Home/EmployeeHome";
        } else {
            const errorMessage = await response.text();
            alert('Failed to update profile: ' + errorMessage);
        }
    });
};

function approveUser(profileUpdateId) {
    // Confirm the approval action
    if (confirm("Are you sure you want to approve this profile update?")) {
        fetch(`/User/ApproveProfileUpdate?profileUpdateId=${profileUpdateId}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            },
        })
            .then(response => {
                if (response.ok) {
                    alert("Profile update approved successfully!");
                    
                    window.location.reload();  // Reload the page to see updated data
                } else {
                    alert("Failed to approve profile update.");
                }
    const updateAdminForm = document.getElementById("UpdateAdminForm");
    if (updateAdminForm) {
        updateAdminForm.addEventListener("submit", async function (event) {
        event.preventDefault(); 
        const newUsername = document.getElementById("newUsername").value;
        const newEmail = document.getElementById("newEmail").value;
        const response = await fetch('/User/EditAdminOwnInfo', {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                NewUsername: newUsername,
                NewEmail: newEmail
            })
        });
        

        if (response.ok) {
            alert('Profile updated');
            window.location.href = "/Home/AdminHome";
        } else {
            const errorMessage = await response.text();
            alert('Failed to update profile: ' + errorMessage);
        }
    });
}
    const updateSuperAdminForm = document.getElementById("UpdateSuperAdminForm");
    if (updateSuperAdminForm) {
        updateSuperAdminForm.addEventListener("submit", async function (event) {
        event.preventDefault();

        const userId = document.getElementById("userId").value;
        const newUsername = document.getElementById("newUsername").value;
        const newEmail = document.getElementById("newEmail").value;

        const response = await fetch(`/User/EditAdminInfo?id=${userId}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                NewUsername: newUsername,
                NewEmail: newEmail
            })
        });

        if (response.ok) {
            alert('Profile updated');
            window.location.href = "/Home/AdminHome";
        } else {
            const errorMessage = await response.text();
            alert('Failed to update profile: ' + errorMessage);
        }
    });
    }
};

function approveUser(profileUpdateId) {
    // Confirm the approval action
    if (confirm("Are you sure you want to approve this profile update?")) {
        fetch(`/User/ApproveProfileUpdate?profileUpdateId=${profileUpdateId}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            },
        })
            .then(response => {
                if (response.ok) {
                    alert("Profile update approved successfully!");

                    window.location.reload();  // Reload the page to see updated data
                } else {
                    alert("Failed to approve profile update.");
                }
            })
            .catch(error => {
                console.error("Error:", error);
                alert("An error occurred while approving the profile update.");
            });
    }
}
