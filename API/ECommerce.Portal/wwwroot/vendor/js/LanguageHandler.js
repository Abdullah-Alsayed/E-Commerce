const arabic = "ar-EG";
const english = "en-US";
let userLanguage = arabic;

function getPreferredLanguage() {
    let language = userLanguage;
    const cookies = document.cookie.split(';');
    for (let cookie of cookies) {
        const [name, value] = cookie.trim().split('=');
        if (name === 'PreferredLanguage') {
            language = value;
        }
    }
    userLanguage = language;
}

// Run on page load
document.addEventListener('DOMContentLoaded', getPreferredLanguage);