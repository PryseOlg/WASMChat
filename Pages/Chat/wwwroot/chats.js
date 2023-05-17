export function scrollContainerDown() {
    let container = document.getElementById("my-container");
    setTimeout(function() {
        container.scrollTop = container.scrollHeight;
    }, 0);
}