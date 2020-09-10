document.addEventListener('DOMContentLoaded', () => {

  // Unix timestamp (in seconds) to count down to
    var d = new Date("august 08, 2020 23:59:00");

    var t = new Date();
    var today = t.getDate();

    var dif = 0;
    if (today > 8) {
        dif = 31 - today + d.getDate();
    } else {
        dif = d.getDate() - today;
    }

    var dt = new Date();
    var secs = dt.getSeconds() + (60 * (dt.getMinutes() + (60 * dt.getHours())));
    var difSecs = 86400 - secs;

    var aaaa = dif;

  var twoDaysFromNow = (new Date().getTime() / 1000) + ((86400 * dif-1)+difSecs) + 1;

  // Set up FlipDown
  var flipdown = new FlipDown(twoDaysFromNow)

    // Start the countdown
    .start()

    // Do something when the countdown ends
    .ifEnded(() => {
      console.log('The countdown has ended!');
    });

  // Toggle theme
  //var interval = setInterval(() => {
  //  let body = document.body;
  //  body.classList.toggle('light-theme');
  //  body.querySelector('#flipdown').classList.toggle('flipdown__theme-dark');
  //  body.querySelector('#flipdown').classList.toggle('flipdown__theme-light');
  //}, 5000);

  // Show version number
  var ver = document.getElementById('ver');
  ver.innerHTML = flipdown.version;
});
