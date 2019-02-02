Username = document.getElementById("Username");
Password = document.getElementById("Password");
AdminsList = document.getElementById("AdminsList");

if (document.location.host == "infomedia.orebro.se")
    RootUrl = "/TGE/";
else
    RootUrl = "/";

function cAlert(text, color) {
    customAlert = document.getElementById("CustomAlert");
    customAlert.style.background = color;
    customAlert.style.top = "0";
    customAlert.style.opacity = "1";
    customAlert.innerHTML = text;
    setTimeout(() => {
        customAlert.style.opacity = "0";
        customAlert.style.top = "-50px";
    }, 2500);
    clearTimeout();
}

function addAdmin() {
    if (Username.value != "" && Password.value != "") {
        $.ajax({
            type: "POST",
            url: RootUrl + "Admin/AddAdmin",
            data: { Username: Username.value, Password: Password.value },
            dataType: "json",
            success: function (data) {
                obj = JSON.parse(data);
                cAlert(obj.Result, obj.Color);
                Username.value = "";
                Password.value = "";
                getAdmins();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                cAlert("Något gick fel\nErrormessage: " + errorThrown, "#e74c3c");
            }
        });
    } else {
        cAlert("Måste ange användarnamn och lösenord", "#e74c3c");
    }

}

function getAdmins() {
    $.ajax({
        type: "POST",
        url: RootUrl + "Admin/GetAdmins",
        dataType: "json",
        success: function (data) {
            obj = JSON.parse(data);
            newAdminsList = "";

            if (obj.length <= 1)
                AdminsList.innerHTML = "Inga extra administratörer, lägg till flera ovan.";
            else {
                for (var i = 1; i < obj.length; i++) {
                    newAdminsList += "<li>" + obj[i].Username + "<button onclick='removeAdmin(" + obj[i].UserId + ")'>Ta bort</button></li>";
                }
                AdminsList.innerHTML = newAdminsList;
            }
        }
    });
}

function removeAdmin(id) {
    $.ajax({
        type: "POST",
        url: RootUrl + "Admin/RemoveAdmin",
        data: { UserId: id },
        dataType: "json",
        success: function () {
            cAlert("Admin borttagen", "#2ecc71");
            getAdmins();
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            cAlert("Något gick fel\nErrormessage: " + errorThrown, "#e74c3c");
        }
    });
}
window.onload = getAdmins();