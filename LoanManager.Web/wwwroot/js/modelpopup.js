(function ($) {
    function SonuSingh() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-base").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal'); 
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new SonuSingh();
        self.init();
    })
}(jQuery))
