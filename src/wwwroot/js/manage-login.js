(function () {
  const toggleButton = document.getElementById("togglePassword");
  const passwordInput = document.getElementById("passwordInput");
  const toggleIcon = document.getElementById("togglePasswordIcon");

  if (!toggleButton || !passwordInput || !toggleIcon) return;

  toggleButton.addEventListener("click", () => {
    const isHidden = passwordInput.type === "password";
    passwordInput.type = isHidden ? "text" : "password";
    toggleIcon.textContent = isHidden ? "visibility" : "visibility_off";
  });
})();
