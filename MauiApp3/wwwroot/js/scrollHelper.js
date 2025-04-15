window.scrollToBottom = () => {
    const el = document.getElementById('messagesEnd');
    if (el && el.scrollIntoView) {
        el.scrollIntoView({ behavior: "smooth" });
    }
};
