using System.Web.Optimization;

namespace ECommerce_Shop
{
    public class BundleConfig
    {

        public static void RegisterBundles(BundleCollection bundles)
        {

            // CSS style (bootstrap/inspinia)
            bundles.Add(new StyleBundle("~/Content/Backend/css").Include(
                      "~/Content/Backend/bootstrap.min.css",
                      "~/Content/Backend/animate.css",
                      "~/Content/Backend/style.css",
                       "~/Content/Backend/custom.css"));

            // Font Awesome icons
            bundles.Add(new StyleBundle("~/font-awesome/Backend/css").Include(
                      "~/fonts/Backend/font-awesome/css/font-awesome.css", new CssRewriteUrlTransform()));

            // jQuery
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/Backend/jquery-3.1.1.min.js"));

            // jQueryUI CSS
            bundles.Add(new ScriptBundle("~/Scripts/Backend/plugins/jquery-ui/jqueryuiStyles").Include(
                        "~/Scripts/Backend/plugins/jquery-ui/jquery-ui.min.css"));

            // jQueryUI 
            bundles.Add(new StyleBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/Backend/plugins/jquery-ui/jquery-ui.min.js"));

            // Bootstrap
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/Backend/bootstrap.min.js"));

            // Inspinia script
            bundles.Add(new ScriptBundle("~/bundles/inspinia").Include(
                      "~/Scripts/Backend/plugins/metisMenu/metisMenu.min.js",
                      "~/Scripts/Backend/plugins/pace/pace.min.js",
                      "~/Scripts/Backend/app/inspinia.js"));

            // Inspinia skin config script
            bundles.Add(new ScriptBundle("~/bundles/skinConfig").Include(
                      "~/Scripts/Backend/app/skin.config.min.js"));

            // SlimScroll
            bundles.Add(new ScriptBundle("~/plugins/slimScroll").Include(
                      "~/Scripts/Backend/plugins/slimscroll/jquery.slimscroll.min.js"));

            // Peity
            bundles.Add(new ScriptBundle("~/plugins/peity").Include(
                      "~/Scripts/Backend/plugins/peity/jquery.peity.min.js"));

            // Video responsible
            bundles.Add(new ScriptBundle("~/plugins/videoResponsible").Include(
                      "~/Scripts/Backend/plugins/video/responsible-video.js"));

            // Lightbox gallery css styles
            bundles.Add(new StyleBundle("~/Content/Backend/plugins/blueimp/css/").Include(
                      "~/Content/Backend/plugins/blueimp/css/blueimp-gallery.min.css"));

            // Lightbox gallery
            bundles.Add(new ScriptBundle("~/plugins/lightboxGallery").Include(
                      "~/Scripts/Backend/plugins/blueimp/jquery.blueimp-gallery.min.js"));

            // Sparkline
            bundles.Add(new ScriptBundle("~/plugins/sparkline").Include(
                      "~/Scripts/Backend/plugins/sparkline/jquery.sparkline.min.js"));

            // Morriss chart css styles
            bundles.Add(new StyleBundle("~/plugins/morrisStyles").Include(
                      "~/Content/Backend/plugins/morris/morris-0.4.3.min.css"));

            // Flat picker css styles
            bundles.Add(new StyleBundle("~/plugins/flatpickrStyle").Include(
                "~/Content/Backend/plugins/flatpickr/flatpickr.min.css"));

            // Flat picker
            bundles.Add(new ScriptBundle("~/plugins/flatpickr").Include(
                "~/Content/Backend/plugins/flatpickr/flatpickr.min.js"));

            // Morriss chart
            bundles.Add(new ScriptBundle("~/plugins/morris").Include(
                      "~/Scripts/Backend/plugins/morris/raphael-2.1.0.min.js",
                      "~/Scripts/Backend/plugins/morris/morris.js"));

            // Flot chart
            bundles.Add(new ScriptBundle("~/plugins/flot").Include(
                      "~/Scripts/Backend/plugins/flot/jquery.flot.js",
                      "~/Scripts/Backend/plugins/flot/jquery.flot.tooltip.min.js",
                      "~/Scripts/Backend/plugins/flot/jquery.flot.resize.js",
                      "~/Scripts/Backend/plugins/flot/jquery.flot.pie.js",
                      "~/Scripts/Backend/plugins/flot/jquery.flot.time.js",
                      "~/Scripts/Backend/plugins/flot/jquery.flot.spline.js"));

            // Rickshaw chart
            bundles.Add(new ScriptBundle("~/plugins/rickshaw").Include(
                      "~/Scripts/Backend/plugins/rickshaw/vendor/d3.v3.js",
                      "~/Scripts/Backend/plugins/rickshaw/rickshaw.min.js"));

            // ChartJS chart
            bundles.Add(new ScriptBundle("~/plugins/chartJs").Include(
                      "~/Scripts/Backend/plugins/chartjs/Chart.min.js"));

            // iCheck css styles
            bundles.Add(new StyleBundle("~/Content/Backend/plugins/iCheck/iCheckStyles").Include(
                      "~/Content/Backend/plugins/iCheck/custom.css"));

            // iCheck
            bundles.Add(new ScriptBundle("~/plugins/iCheck").Include(
                      "~/Scripts/Backend/plugins/iCheck/icheck.min.js"));

            // dataTables css styles
            bundles.Add(new StyleBundle("~/Content/Backend/plugins/dataTables/dataTablesStyles").Include(
                      "~/Content/Backend/plugins/dataTables/datatables.min.css"));

            // dataTables 
            bundles.Add(new ScriptBundle("~/plugins/dataTables").Include(
                      "~/Scripts/Backend/plugins/dataTables/datatables.min.js"));

            // jeditable 
            bundles.Add(new ScriptBundle("~/plugins/jeditable").Include(
                      "~/Scripts/Backend/plugins/jeditable/jquery.jeditable.js"));

            // jqGrid styles
            bundles.Add(new StyleBundle("~/Content/Backend/plugins/jqGrid/jqGridStyles").Include(
                      "~/Content/Backend/plugins/jqGrid/ui.jqgrid.css"));

            // jqGrid 
            bundles.Add(new ScriptBundle("~/plugins/jqGrid").Include(
                      "~/Scripts/Backend/plugins/jqGrid/i18n/grid.locale-en.js",
                      "~/Scripts/Backend/plugins/jqGrid/jquery.jqGrid.min.js"));

            // codeEditor styles
            bundles.Add(new StyleBundle("~/plugins/codeEditorStyles").Include(
                      "~/Content/Backend/plugins/codemirror/codemirror.css",
                      "~/Content/Backend/plugins/codemirror/ambiance.css"));

            // codeEditor 
            bundles.Add(new ScriptBundle("~/plugins/codeEditor").Include(
                      "~/Scripts/Backend/plugins/codemirror/codemirror.js",
                      "~/Scripts/Backend/plugins/codemirror/mode/javascript/javascript.js"));

            // codeEditor 
            bundles.Add(new ScriptBundle("~/plugins/nestable").Include(
                      "~/Scripts/Backend/plugins/nestable/jquery.nestable.js"));

            // validate 
            bundles.Add(new ScriptBundle("~/plugins/validate").Include(
                      "~/Scripts/Backend/plugins/validate/jquery.validate.min.js"));

            // fullCalendar styles
            bundles.Add(new StyleBundle("~/plugins/fullCalendarStyles").Include(
                      "~/Content/Backend/plugins/fullcalendar/fullcalendar.css"));

            // fullCalendar 
            bundles.Add(new ScriptBundle("~/plugins/fullCalendar").Include(
                      "~/Scripts/Backend/plugins/fullcalendar/moment.min.js",
                      "~/Scripts/Backend/plugins/fullcalendar/fullcalendar.min.js"));

            // vectorMap 
            bundles.Add(new ScriptBundle("~/plugins/vectorMap").Include(
                      "~/Scripts/Backend/plugins/jvectormap/jquery-jvectormap-1.2.2.min.js",
                      "~/Scripts/Backend/plugins/jvectormap/jquery-jvectormap-world-mill-en.js"));

            // ionRange styles
            bundles.Add(new StyleBundle("~/Content/Backend/plugins/ionRangeSlider/ionRangeStyles").Include(
                      "~/Content/Backend/plugins/ionRangeSlider/ion.rangeSlider.css",
                      "~/Content/Backend/plugins/ionRangeSlider/ion.rangeSlider.skinFlat.css"));

            // ionRange 
            bundles.Add(new ScriptBundle("~/plugins/ionRange").Include(
                      "~/Scripts/Backend/plugins/ionRangeSlider/ion.rangeSlider.min.js"));

            // dataPicker styles
            bundles.Add(new StyleBundle("~/plugins/dataPickerStyles").Include(
                      "~/Content/Backend/plugins/datapicker/datepicker3.css"));

            // dataPicker 
            bundles.Add(new ScriptBundle("~/plugins/dataPicker").Include(
                      "~/Scripts/Backend/plugins/datapicker/bootstrap-datepicker.js"));

            // nouiSlider styles
            bundles.Add(new StyleBundle("~/plugins/nouiSliderStyles").Include(
                      "~/Content/Backend/plugins/nouslider/jquery.nouislider.css"));

            // nouiSlider 
            bundles.Add(new ScriptBundle("~/plugins/nouiSlider").Include(
                      "~/Scripts/Backend/plugins/nouslider/jquery.nouislider.min.js"));

            // jasnyBootstrap styles
            bundles.Add(new StyleBundle("~/plugins/jasnyBootstrapStyles").Include(
                      "~/Content/Backend/plugins/jasny/jasny-bootstrap.min.css"));

            // jasnyBootstrap 
            bundles.Add(new ScriptBundle("~/plugins/jasnyBootstrap").Include(
                      "~/Scripts/Backend/plugins/jasny/jasny-bootstrap.min.js"));

            // switchery styles
            bundles.Add(new StyleBundle("~/plugins/switcheryStyles").Include(
                      "~/Content/Backend/plugins/switchery/switchery.css"));

            // switchery 
            bundles.Add(new ScriptBundle("~/plugins/switchery").Include(
                      "~/Scripts/Backend/plugins/switchery/switchery.js"));

            // chosen styles
            bundles.Add(new StyleBundle("~/Content/Backend/plugins/chosen/chosenStyles").Include(
                      "~/Content/Backend/plugins/chosen/bootstrap-chosen.css"));

            // chosen 
            bundles.Add(new ScriptBundle("~/plugins/chosen").Include(
                      "~/Scripts/Backend/plugins/chosen/chosen.jquery.js"));

            // knob 
            bundles.Add(new ScriptBundle("~/plugins/knob").Include(
                      "~/Scripts/Backend/plugins/jsKnob/jquery.knob.js"));

            // wizardSteps styles
            bundles.Add(new StyleBundle("~/plugins/wizardStepsStyles").Include(
                      "~/Content/Backend/plugins/steps/jquery.steps.css"));

            // wizardSteps 
            bundles.Add(new ScriptBundle("~/plugins/wizardSteps").Include(
                      "~/Scripts/Backend/plugins/steps/jquery.steps.min.js"));

            // dropZone styles
            bundles.Add(new StyleBundle("~/Content/Backend/plugins/dropzone/dropZoneStyles").Include(
                      "~/Content/Backend/plugins/dropzone/basic.css",
                      "~/Content/Backend/plugins/dropzone/dropzone.css"));

            // dropZone 
            bundles.Add(new ScriptBundle("~/plugins/dropZone").Include(
                      "~/Scripts/Backend/plugins/dropzone/dropzone.js"));

            // summernote styles
            bundles.Add(new StyleBundle("~/plugins/summernoteStyles").Include(
                      "~/Content/Backend/plugins/summernote/summernote.css",
                      "~/Content/Backend/plugins/summernote/summernote-bs3.css"));

            // summernote 
            bundles.Add(new ScriptBundle("~/plugins/summernote").Include(
                      "~/Scripts/Backend/plugins/summernote/summernote.min.js"));

            // toastr notification 
            bundles.Add(new ScriptBundle("~/plugins/toastr").Include(
                      "~/Scripts/Backend/plugins/toastr/toastr.min.js"));

            // toastr notification styles
            bundles.Add(new StyleBundle("~/plugins/toastrStyles").Include(
                      "~/Content/Backend/plugins/toastr/toastr.min.css"));

            // color picker
            bundles.Add(new ScriptBundle("~/plugins/colorpicker").Include(
                      "~/Scripts/Backend/plugins/colorpicker/bootstrap-colorpicker.min.js"));

            // color picker styles
            bundles.Add(new StyleBundle("~/Content/Backend/plugins/colorpicker/colorpickerStyles").Include(
                      "~/Content/Backend/plugins/colorpicker/bootstrap-colorpicker.min.css"));

            // image cropper
            bundles.Add(new ScriptBundle("~/plugins/imagecropper").Include(
                      "~/Scripts/Backend/plugins/cropper/cropper.min.js"));

            // image cropper styles
            bundles.Add(new StyleBundle("~/plugins/imagecropperStyles").Include(
                      "~/Content/Backend/plugins/cropper/cropper.min.css"));



            // jsTree
            bundles.Add(new ScriptBundle("~/plugins/jsTree").Include(
                      "~/Scripts/Backend/plugins/jsTree/jstree.min.js"));

            // jsTree styles
            bundles.Add(new StyleBundle("~/Content/Backend/plugins/jsTree").Include(
                      "~/Content/Backend/plugins/jsTree/style.css"));

            // Diff
            bundles.Add(new ScriptBundle("~/plugins/diff").Include(
                      "~/Scripts/Backend/plugins/diff_match_patch/javascript/diff_match_patch.js",
                      "~/Scripts/Backend/plugins/preetyTextDiff/jquery.pretty-text-diff.min.js"));

            // Idle timer
            bundles.Add(new ScriptBundle("~/plugins/idletimer").Include(
                      "~/Scripts/Backend/plugins/idle-timer/idle-timer.min.js"));

            // Tinycon
            bundles.Add(new ScriptBundle("~/plugins/tinycon").Include(
                      "~/Scripts/Backend/plugins/tinycon/tinycon.min.js"));

            // Chartist
            bundles.Add(new StyleBundle("~/plugins/chartistStyles").Include(
                      "~/Content/Backend/plugins/chartist/chartist.min.css"));

            // jsTree styles
            bundles.Add(new ScriptBundle("~/plugins/chartist").Include(
                      "~/Scripts/Backend/plugins/chartist/chartist.min.js"));

            // Awesome bootstrap checkbox
            bundles.Add(new StyleBundle("~/plugins/awesomeCheckboxStyles").Include(
                      "~/Content/Backend/plugins/awesome-bootstrap-checkbox/awesome-bootstrap-checkbox.css"));

            // Clockpicker styles
            bundles.Add(new StyleBundle("~/plugins/clockpickerStyles").Include(
                      "~/Content/Backend/plugins/clockpicker/clockpicker.css"));

            // Clockpicker
            bundles.Add(new ScriptBundle("~/plugins/clockpicker").Include(
                      "~/Scripts/Backend/plugins/clockpicker/clockpicker.js"));

            // Date range picker Styless
            bundles.Add(new StyleBundle("~/plugins/dateRangeStyles").Include(
                      "~/Content/Backend/plugins/daterangepicker/daterangepicker-bs3.css"));

            // Date range picker
            bundles.Add(new ScriptBundle("~/plugins/dateRange").Include(
                      // Date range use moment.js same as full calendar plugin 
                      "~/Scripts/Backend/plugins/fullcalendar/moment.min.js",
                      "~/Scripts/Backend/plugins/daterangepicker/daterangepicker.js"));

            // Sweet alert Styless
            bundles.Add(new StyleBundle("~/plugins/sweetAlertStyles").Include(
                      "~/Content/Backend/plugins/sweetalert/sweetalert.css"));

            // Sweet alert
            bundles.Add(new ScriptBundle("~/plugins/sweetAlert").Include(
                      "~/Scripts/Backend/plugins/sweetalert/sweetalert.min.js"));

            // Footable Styless
            bundles.Add(new StyleBundle("~/plugins/footableStyles").Include(
                      "~/Content/Backend/plugins/footable/footable.core.css", new CssRewriteUrlTransform()));

            // Footable alert
            bundles.Add(new ScriptBundle("~/plugins/footable").Include(
                      "~/Scripts/Backend/plugins/footable/footable.all.min.js"));

            // Select2 Styless
            bundles.Add(new StyleBundle("~/plugins/select2Styles").Include(
                      "~/Content/Backend/plugins/select2/select2.min.css"));

            // Select2
            bundles.Add(new ScriptBundle("~/plugins/select2").Include(
                      "~/Scripts/Backend/plugins/select2/select2.full.min.js"));

            // Masonry
            bundles.Add(new ScriptBundle("~/plugins/masonry").Include(
                      "~/Scripts/Backend/plugins/masonary/masonry.pkgd.min.js"));

            // Slick carousel Styless
            bundles.Add(new StyleBundle("~/plugins/slickStyles").Include(
                      "~/Content/Backend/plugins/slick/slick.css", new CssRewriteUrlTransform()));

            // Slick carousel theme Styless
            bundles.Add(new StyleBundle("~/plugins/slickThemeStyles").Include(
                      "~/Content/Backend/plugins/slick/slick-theme.css", new CssRewriteUrlTransform()));

            // Slick carousel
            bundles.Add(new ScriptBundle("~/plugins/slick").Include(
                      "~/Scripts/Backend/plugins/slick/slick.min.js"));

            // Ladda buttons Styless
            bundles.Add(new StyleBundle("~/plugins/laddaStyles").Include(
                      "~/Content/Backend/plugins/ladda/ladda-themeless.min.css"));

            // Ladda buttons
            bundles.Add(new ScriptBundle("~/plugins/ladda").Include(
                      "~/Scripts/Backend/plugins/ladda/spin.min.js",
                      "~/Scripts/Backend/plugins/ladda/ladda.min.js",
                      "~/Scripts/Backend/plugins/ladda/ladda.jquery.min.js"));

            // Dotdotdot buttons
            bundles.Add(new ScriptBundle("~/plugins/truncate").Include(
                      "~/Scripts/Backend/plugins/dotdotdot/jquery.dotdotdot.min.js"));

            // Touch Spin Styless
            bundles.Add(new StyleBundle("~/plugins/touchSpinStyles").Include(
                      "~/Content/Backend/plugins/touchspin/jquery.bootstrap-touchspin.min.css"));

            // Touch Spin
            bundles.Add(new ScriptBundle("~/plugins/touchSpin").Include(
                      "~/Scripts/Backend/plugins/touchspin/jquery.bootstrap-touchspin.min.js"));

            // Tour Styless
            bundles.Add(new StyleBundle("~/plugins/tourStyles").Include(
                      "~/Content/Backend/plugins/bootstrapTour/bootstrap-tour.min.css"));

            // Tour Spin
            bundles.Add(new ScriptBundle("~/plugins/tour").Include(
                      "~/Scripts/Backend/plugins/bootstrapTour/bootstrap-tour.min.js"));

            // i18next Spin
            bundles.Add(new ScriptBundle("~/plugins/i18next").Include(
                      "~/Scripts/Backend/plugins/i18next/i18next.min.js"));

            // ckeditor
            bundles.Add(new ScriptBundle("~/plugins/ckeditor").Include(
                      "~/Scripts/Backend/plugins/ckeditor/ckeditor.js"));

            // Clipboard Spin
            bundles.Add(new ScriptBundle("~/plugins/clipboard").Include(
                      "~/Scripts/Backend/plugins/clipboard/clipboard.min.js"));

            // c3 Styless
            bundles.Add(new StyleBundle("~/plugins/c3Styles").Include(
                      "~/Content/Backend/plugins/c3/c3.min.css"));

            // c3 Charts
            bundles.Add(new ScriptBundle("~/plugins/c3").Include(
                      "~/Scripts/Backend/plugins/c3/c3.min.js"));

            // D3
            bundles.Add(new ScriptBundle("~/plugins/d3").Include(
                      "~/Scripts/Backend/plugins/d3/d3.min.js"));

            // Markdown Styless
            bundles.Add(new StyleBundle("~/plugins/markdownStyles").Include(
                      "~/Content/Backend/plugins/bootstrap-markdown/bootstrap-markdown.min.css"));

            // Markdown 
            bundles.Add(new ScriptBundle("~/plugins/markdown").Include(
                      "~/Scripts/Backend/plugins/bootstrap-markdown/bootstrap-markdown.js",
                      "~/Scripts/Backend/plugins/bootstrap-markdown/markdown.js"));

            // Datamaps
            bundles.Add(new ScriptBundle("~/plugins/datamaps").Include(
                      "~/Scripts/Backend/plugins/topojson/topojson.js",
                      "~/Scripts/Backend/plugins/datamaps/datamaps.all.min.js"));

            // Taginputs Styless
            bundles.Add(new StyleBundle("~/plugins/tagInputsStyles").Include(
                      "~/Content/Backend/plugins/bootstrap-tagsinput/bootstrap-tagsinput.css"));

            // Taginputs
            bundles.Add(new ScriptBundle("~/plugins/tagInputs").Include(
                      "~/Scripts/Backend/plugins/bootstrap-tagsinput/bootstrap-tagsinput.js"));

            // Duallist Styless
            bundles.Add(new StyleBundle("~/plugins/duallistStyles").Include(
                      "~/Content/Backend/plugins/bootstrap-tagsinput/bootstrap-tagsinput.css"));

            // Duallist
            bundles.Add(new ScriptBundle("~/plugins/duallist").Include(
                      "~/Scripts/Backend/plugins/dualListbox/jquery.bootstrap-duallistbox.js"));

            // SocialButtons styles
            bundles.Add(new StyleBundle("~/plugins/socialButtonsStyles").Include(
                      "~/Content/Backend/plugins/bootstrapSocial/bootstrap-social.css"));

            // Typehead
            bundles.Add(new ScriptBundle("~/plugins/typehead").Include(
                      "~/Scripts/Backend/plugins/typehead/bootstrap3-typeahead.min.js"));

            // Pdfjs
            bundles.Add(new ScriptBundle("~/plugins/pdfjs").Include(
                      "~/Scripts/Backend/plugins/pdfjs/pdf.js"));

            // Touch Punch 
            bundles.Add(new StyleBundle("~/plugins/touchPunch").Include(
                        "~/Scripts/Backend/plugins/touchpunch/jquery.ui.touch-punch.min.js"));

            // WOW 
            bundles.Add(new StyleBundle("~/plugins/wow").Include(
                        "~/Scripts/Backend/plugins/wow/wow.min.js"));

            // Text spinners styles
            bundles.Add(new StyleBundle("~/plugins/textSpinnersStyles").Include(
                      "~/Content/Backend/plugins/textSpinners/spinners.css"));

            // Password meter 
            bundles.Add(new StyleBundle("~/plugins/passwordMeter").Include(
                        "~/Scripts/Backend/plugins/pwstrength/pwstrength-bootstrap.min.js",
                        "~/Scripts/Backend/plugins/pwstrength/zxcvbn.js"));




        }
    }
}