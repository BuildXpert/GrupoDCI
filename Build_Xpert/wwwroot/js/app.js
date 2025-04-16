function startApp() {
    inventoryItemSent();
    disableDefaultOption();
    console.log("Injected!")
}

function inventoryItemSent() {
    var containers = document.querySelectorAll(".inventoryFormContainer");
    console.log(containers)
    if (containers) {
        containers.forEach(container => {
            container.addEventListener("click", function (event) {
                if (event.target && event.target.classList.contains("submitBtn")) {
                    event.target.textContent = "Registrado!";
                    console.log("Registrado");
                    console.log(event.target);
                } else {
                    console.log("No");
                }
            });
        });
    };
};

function disableDefaultOption() {
    var containers = document.querySelectorAll(".inventoryFormContainer");
    containers.forEach(container => {
        container.addEventListener("click", function (event) {
            if (event.target && event.target.classList.contains("dropdownOptions")) {
                const defaultOption = event.target.querySelector("#defaultOption");
                if (defaultOption) defaultOption.disabled = true;
            }
        });
    });
}

window.onload = startApp;