window.noise = window.noise || {};

(function (noise, $, undefined) {
    function getNote(i) {
        switch (i) {
            case 0:
                return 'A';
                break;
            case 1:
                return 'B';
                break;
            case 2:
                return 'C';
                break;
            case 3:
                return 'D';
                break;
            case 4:
                return 'E';
                break;
            case 5:
                return 'F';
                break;
            case 6:
                return 'G';
                break;
            default:
                return '!';
                break;
        }
    }

    function getIntonation(i) {
        switch (i) {
            case -1:
                return 'b';
                break;
            case 0:
                return '';
                break;
            case 1:
                return '#';
                break;
        }
    }

    noise.init = function() {    
        $("#generate-wave").click(function () {
            playStuff();
        });
        $("#merge-stuff").click(function() {
            playUrl("/home/merge?");
        });
        $("#random").click(function() {
            playUrl("/home/random?");
        });
         $("#random-with-seed").click(function() {
            playUrl("/home/random?seed=" + $("#seed").val());
        });
        $("body").keyup(function (event) {
            if (event.keyCode == '13') {
                playStuff();
            }
        });

        for (var octave = 1; octave <= 8; octave++) { //octaves;
            for (var note = 0; note <= 6; note++) { //notes;
                for (var intonation = -1; intonation < 1; intonation++) {
                    var skip = (intonation === -1 && (note === 5 || note === 2))
                    if (!skip) {
                        var markup = "<button data-note='" + note + "' data-octave='" + octave + "' data-intonation='" + intonation + "' class='note-button'>"
                        markup += getNote(note) + octave + getIntonation(intonation) + "</button>";
                        if (intonation === -1) {
                            markup = '<div class="black-keys">' + markup + '</div>';
                        }
                        $("#keyboard").append(markup);
                    }
                }
            }
        }        
        $(".note-button").click(function () {
            var $this = $(this);
            var note = $this.data('note');
            var octave = $this.data('octave');
            var intonation = $this.data('intonation');
            var url = "/home/note?note=" + note + "&octave=" + octave + "&intonation=" + intonation;
            playUrl(url);
        });
    }

    function playStuff() {
        var url = "/home/audio?freq=" + $("#freq").val();
        playUrl(url);
    }

    function playUrl(url) {
        url += "&wave=" + $("#wavetype").val() + "&ms=" + $("#duration").val();
        $.get(url).success(function (data) {
            $('audio').each(function () {
                $(this).get(0).pause();
                $(this).attr('src', '');
            });

            var m = '<audio controls preload="auto" src="' + data.audio + '">';
            $('#container').empty().append(m);
            $("audio")[0].play();
            console.log(data.d);
        });
    }
}(window.noise = window.noise || {}, jQuery));