
document.addEventListener("DOMContentLoaded", function () {
    const fileInput = document.getElementById("xmlFile");
    const importBtn = document.getElementById("importBtn");

    fileInput.addEventListener("change", () => {
        // Check if a file is selected
        importBtn.style.display = fileInput.files.length > 0 ? "block" : "none";
    });
});
async function uploadFile() {
    try {
        const fileInput = document.getElementById("xmlFile");

        if (!fileInput.files[0]) {
            alert("Please select a file to upload.");
            return;
        }

        const fileName = fileInput.files[0].name.toLowerCase();

        if (!fileName.endsWith(".xml")) {
            alert("Please select an XML file.");
            return;
        }

        const xmlFile = new FormData();
        xmlFile.append("xmlFile", fileInput.files[0]);

        const response = await fetch('/Home/ImportXml', {
            method: 'POST',
            body: xmlFile,
        });

        if (!response.ok) {
            throw new Error('Network response was not ok');
        }

        const redirectUrl = response.headers.get('Location');
        if (redirectUrl) {
            window.location.href = redirectUrl; // Redirect to the URL provided by the server
        } else {
            // Handle other scenarios as needed (e.g., show a success message, reload the page, etc.)
            console.log('Server did not provide a redirect URL');
            window.location.reload(); // Reload the current page
        }

    } catch (error) {
        console.error('There has been a problem with your fetch operation:', error);
        alert("An error occurred during import. Please try again.");
    }
}
