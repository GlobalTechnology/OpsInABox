(function ($, Sys) {



    $(document).ready(function () {
              $('.numeric').numeric();


        $('.aButton').button();

        //Startup Routine
        $('.monthly').each(function () { setMinMax($(this)); });
        $('.yearly').each(function () { setMinMax($(this)) });
        $('.monthly.net').each(function () {
            var f = $(this).siblings("input:hidden").val();

            f = f.replace(/\{NET}/g, $(this).val() == '' ? '0' : $(this).val());
            f = replaceTags(f);

            $(this).parent().find(".net-tax-month").text(eval(f).toFixed(0));

            $(this).parent().parent().siblings().find('.net-tax-year').text((eval(f) * 12.0).toFixed(0));

            $(this).siblings('.net-tax').find("input[type=hidden]").val(eval(f).toFixed(0));
        });

        handleFormulas();

        $('.sectionTotal').each(function () {
            calculateSectionTotal($(this).parent().parent().parent().parent());
        });

        $('#' + startPeriodId).change(function (e) {
            if ($(this).val() == "")
                $('#' + customDateId).show();
            else
                $('#' + customDateId).hide();
        });

        //Event Handlers
        $('.monthly').keyup(function () {
            //SetValidation
            setMinMax($(this));




            if ($(this).val().length > 0)
                $(this).parent().parent().siblings().find('.yearly').val((parseFloat($(this).val()) * 12).toFixed(0));
            else $(this).parent().parent().siblings().find('.yearly').val('');


            handleFormulas();
            if ($(this).hasClass('net')) {
                var f = $(this).siblings("input:hidden").val();

                f = f.replace(/\{NET}/g, $(this).val() == '' ? '0' : $(this).val());
                f = replaceTags(f);

                // alert(f);
                $(this).parent().find(".net-tax-month").text(eval(f).toFixed(0));

                $(this).parent().parent().siblings().find('.net-tax-year').text((eval(f) * 12.0).toFixed(0));

                $(this).siblings('.net-tax').find("input[type=hidden]").val(eval(f).toFixed(0));

            }
            calculateSectionTotal($(this).parent().parent().parent().parent().parent().parent());

        });





        $('.yearly').keyup(function () {
            setMinMax($(this));
            var monthly = $(this).parent().parent().siblings().find('.monthly');
            if ($(this).val().length > 0)
                $(monthly).val((parseFloat($(this).val()) / 12).toFixed(0));
            else $(monthly).val('');


            if ($(this).hasClass('net')) {
                var f = $(monthly).siblings("input:hidden").val();

                f = f.replace("{NET}", $(monthly).val());

                $(monthly).parent().find(".net-tax-month").text(eval(f).toFixed(0));

                $(this).parent().find(".net-tax-year").text((eval(f) * 12.0).toFixed(0));
                $(monthly).parent().find(".net-tax-month").siblings("input:hidden").val(eval(f).toFixed(0));

            }
            calculateSectionTotal($(this).parent().parent().parent().parent().parent().parent());

        });




        $('#' + complienceId).change(function () {

            if (this.checked)
                $('#' + btnSubmitId).removeAttr("disabled");
            else
                $('#' + btnSubmitId).attr("disabled", "disabled");
        });



        $('.mpd-tax-detail input').keyup(function () {

            var ddl = $(this).parent().parent().parent().parent().parent().find('.mpd-tax-mode');
            var m = $(ddl).val();

            var f = '';
            if (m == 'FIXED_RATE') {
                var r = $(ddl).siblings('.mpd-tax-rate').find('.rate').val();
                f = '(({NET}*12) / ( ( 100 / ' + r + ' ) -1 ))/12';
                $(ddl).parent().find('.tax-formula').text(f);
                $(ddl).parent().find('.tax-formula').siblings("input:hidden").val(f);
            }
            else if (m == 'FIXED_AMOUNT') {
                f = $(ddl).siblings('.mpd-tax-fixed').find('.fixed').val();
                $(ddl).parent().find('.tax-formula').text(f);
                $(ddl).parent().find('.tax-formula').siblings("input:hidden").val(f);
            }
            else if (m == 'ALLOWANCE') {
                //TODO: Generate Allowance Formula
                // $(this).siblings('.mpd-tax-allowance').show();
                var r = $(ddl).siblings('.mpd-tax-allowance').find('.rate').val();
                var th = $(ddl).siblings('.mpd-tax-allowance').find('.threshold').val();
                f = 'Math.max((({NET}*12)-' + th + '/ ( ( 100 / ' + r + ' ) -1 ),0)/12)';
                $(ddl).parent().find('.tax-formula').text(f);
                $(ddl).parent().find('.tax-formula').siblings("input:hidden").val(f);

            }
            else if (m == 'BANDS') {

                var r1 = $(ddl).siblings('.mpd-tax-bands').find('.rate1').val();

                var th1 = $(ddl).siblings('.mpd-tax-bands').find('.threshold1').val();
                var r2 = $(ddl).siblings('.mpd-tax-bands').find('.rate2').val();
                var th2 = $(ddl).siblings('.mpd-tax-bands').find('.threshold2').val();
                var r3 = $(ddl).siblings('.mpd-tax-bands').find('.rate3').val();
                var th3 = $(ddl).siblings('.mpd-tax-bands').find('.threshold3').val();
                var r4 = $(ddl).siblings('.mpd-tax-bands').find('.rate4').val();


                if (r1 != '') {
                    if (th1 == '')
                        f = r1 != 0 ? '({NET}*12) / ( ( 100 / ' + r1 + ' ) -1 )' : '0 ';
                    else {
                        f = r1 != 0 ? 'Math.max(Math.min(({NET}*12) / ( ( 100 / ' + r1 + ' ) -1 ),(' + th1 + ')/( ( 100 / ' + r1 + ' ) -1 )),0) ' : '0';
                        if (r2 != '') {
                            if (th2 == '')
                                f += r2 != 0 ? ' + Math.max( (({NET}*12) - ' + th1 + ') / ( ( 100 / ' + r2 + ' ) -1 ),0) ' : '';
                            else {
                                f += r2 != 0 ? ' + Math.max(Math.min((({NET}*12) - ' + th1 + ') / ( ( 100 / ' + r2 + ' ) -1 ),( ' + th2 + ' - ' + th1 + ')/( ( 100 / ' + r2 + ' ) -1 )),0) ' : '';
                                if (r3 != '') {
                                    if (th3 == '')
                                        f += r3 != 0 ? ' + Math.max( (({NET}*12) - ' + th2 + ') / ( ( 100 / ' + r3 + ' ) -1 ),0) ' : '';
                                    else {
                                        f += r3 != 0 ? ' + Math.max(Math.min((({NET}*12)- ' + th2 + ') / ( ( 100 / ' + r3 + ' ) -1 ),( ' + th3 + ' - ' + th2 + ')/( ( 100 / ' + r3 + ' ) -1 )),0) ' : '';
                                        if (r4 != '') {

                                            f += r4 != 0 ? ' + Math.max( (({NET}*12) - ' + th3 + ') / ( ( 100 / ' + r4 + ' ) -1 ),0) ' : '';

                                        }

                                    }
                                }

                            }
                        }
                    }
                }
                f = '(' + f + ')/12';


                $(ddl).parent().find('.tax-formula').text(f);
                $(ddl).parent().find('.tax-formula').siblings("input:hidden").val(f);
            }
            else if (m == 'Custom') {
                //Do Validation?
                $(this).siblings("input:hidden").val(f);
            }



        })

        $('.mpd-edit-mode').change(function () {


            var m = $(this).val();
            $(this).parent().parent().parent().parent().parent().siblings().find('.mpd-edit-mode-detail').hide();
            $(this).parent().parent().parent().parent().parent().siblings().find('.mpd-edit-mode-detail').find('.mpd-tax-mode').change()
            if (m == 'CALCULATED') {
                $(this).parent().parent().parent().parent().parent().siblings().find('.mpd-edit-formula').show();
            }
            else if (m == 'NET_MONTH' || m == 'NET_YEAR') {
                $(this).parent().parent().parent().parent().parent().siblings().find('.mpd-edit-net').show();
            }

        });

        $('.mpd-tax-mode').change(function () {
            $(this).parent().find('.tax-formula').attr('readonly', 'readonly');

            var m = $(this).val();

            $(this).siblings('.mpd-tax-detail').hide();
            if (m == 'FIXED_RATE') {
                $(this).siblings('.mpd-tax-rate').show();
                $(this).parent().find('.tax-formula').text('');
                $(this).parent().find('.tax-formula').siblings("input:hidden").val('');
                $(this).siblings('.mpd-tax-rate').find('.rate').keyup();
            }
            else if (m == 'FIXED_AMOUNT') {
                $(this).siblings('.mpd-tax-fixed').show();
                $(this).parent().find('.tax-formula').text('')
                $(this).parent().find('.tax-formula').siblings("input:hidden").val('');
                $(this).siblings('.mpd-tax-fixed').find('.fixed').keyup();
            }
            else if (m == 'ALLOWANCE') {
                $(this).siblings('.mpd-tax-allowance').show();
                $(this).parent().find('.tax-formula').text('')
                $(this).parent().find('.tax-formula').siblings("input:hidden").val('');
                $(this).siblings('.mpd-tax-allowance').find('.rate').keyup();
            }
            else if (m == 'BANDS') {
                $(this).siblings('.mpd-tax-bands').show();
                $(this).parent().find('.tax-formula').text('')
                $(this).parent().find('.tax-formula').siblings("input:hidden").val('');
                $(this).siblings('.mpd-tax-bands').find('.rate1').keyup();
            }
            else if (m == 'Custom') {
                $(this).siblings('.tax-custom-help').show();
                $(this).parent().find('.tax-formula').removeAttr('readonly');

            }


        });

        $('.edit-cancel').click(function () {
            $('.mpd-edit').hide("slow");
            $('.btn-edit').show();
            $('.btn-insert').show();
        });

        $('.btn-edit,.btn-insert').click(function () {

            $('.mpd-edit').hide("slow");

            $(this).parent().parent().siblings('.mpd-edit').find('.mpd-edit-mode').change();


            $(this).parent().parent().siblings('.mpd-edit').show("slow");




            $('.btn-edit,.btn-insert').show();

            $(this).hide();
        });

        $('.btn-section-insert').click(function () {
            $('.mpd-section-insert').show("slow");
            $(this).hide();
        });
        $('.insert-cancel').click(function () {
            $('.mpd-section-insert').hide("slow");
            $('.btn-section-insert').show();

        });
        $('.btn-edit-section').click(function () {
            $(this).siblings('.mpd-edit-section').show();
            $(this).siblings('.mpd-section-title').hide();
            $(this).hide();
        });
        $('.btn-edit-section-cancel').click(function () {
            $(this).parent().siblings('.btn-edit-section').show();
            $(this).parent().siblings('.mpd-section-title').show();
            $(this).parent().hide();
        });
    });




    //alert(replaceTags('Age: {AGE}; Age2: {AGE2}; StaffType: {STAFFTYPE}; IsCouple: {ISCOUPLE};'));
}(jQuery, window.Sys));

function calculateSectionTotal(section) {
    var sum = 0.0;
    section.find('.yearly').each(function (i, n) {
        if ($(n).val().length > 0)
            sum += parseFloat($(n).val().replace(/\,/g, ''))

    });
    section.find('.net-tax-year').each(function (i, n) {
        if ($(n).text().length > 0)
            sum += parseFloat($(n).text().replace(/\,/g, ''))

    });

    section.find('.section-total-yearly').text(sum.toFixed(0));
    section.find('.section-total-monthly').text((sum / 12).toFixed(0));


    sum = 0.0;
    $('.sectionTotal').each(function (i, n) {
        if ($(n).text().length > 0)
            sum += parseFloat($(n).text().replace(/\,/g, ''))

    });
    var st = (sum / 12)
    $('.subtotal').text(st.toFixed(0));


    var assess = $('#' + assessmentId).val();
    var a = 0.0;
    var a1 = 0.0;
    if (assess.indexOf("%") == 0) {
        a = parseFloat(assess.substring(1)) / 100;
        a1 = (st * a / (1 - a));
    }
    else {
      //  alert(assess);
        a1 = eval(replacePercTags(assess, st, st));

    }






    $('.assessment').text(a1.toFixed(0));
    if (a1 == 0.0) $('.assessmentRow').hide();
    else $('.assessmentRow').show();




    var g = st + a1
    $('.mpdGoal').text(g.toFixed(0));



    $('#' + mpdGoalId).val(g.toFixed(0));



    var comp = $('#' + compensationId).val();

    var c = 0.0;
    var c1 = 0.0;
    if (comp.indexOf("%") == 0) {
        c = parseFloat(comp.substring(1)) / 100;
        c1 = (g * c);
    }
    else if (comp != "") {

        c1 = eval(replacePercTags(comp, st, g));
    }

    $('#' + compensationValueId).val(c1);

    if (c1 == 0.0) $('.myPortionRow').hide();
    else $('.myPortionRow').show();

    var myPortion = g - c1;

    $('.myPortion').text(myPortion.toFixed(0));


    var current = parseFloat($('.currentSupport').val().replace(/\,/g, ''));

    var rem = myPortion - current
    $('.remaining').text(rem.toFixed(0));

    var p = current * 100 / myPortion;
    if (p < 5000)
        $('.percentage').text(p.toFixed(1) + '%');
    else
        $('.percentage').text('');
}

function handleFormulas() {
    $('.calculated').each(function () {
        //Go through each formula and refresh the values
        var f = $(this).siblings("input:hidden").val();
        f = replaceTags(f);
        // console.log(f);
        try {
            $(this).val(eval(f).toFixed(0));
        } catch (e) {
            $(this).val('0');
        }
       



        if ($(this).val().length > 0) {
            $(this).parent().parent().siblings().find('.yearly').val((parseFloat($(this).val()) * 12).toFixed(0));
            $(this).parent().parent().siblings("input:hidden").val((parseFloat($(this).val())).toFixed(0));
        }

        calculateSectionTotal($(this).parent().parent().parent().parent().parent());

    });
}
function replaceTags(f) {
    //Replace ItemValue Taxs {1.1}
    if (f == undefined)
        return '';
    $('.version-number').each(function () {
        var v = $(this).parent().find('.monthly').val();
        if (v != undefined && $(this).text() != undefined) {
            v = v == '' ? 0 : v;


            f = f.replace(new RegExp('{' + $(this).text() + '}', 'g'), v);
        }

    });

    //Replace Age Tag {AGE}


    if (age > 0) f = f.replace(/{AGE}/g, age);

    if (isCouple == 'True') {

        if (age2 > 0) f = f.replace(/{AGE2}/g, age2);
    }

    f = f.replace(/{STAFFTYPE}/g, staffType);
    f = f.replace(/{ISCOUPLE}/g, isCouple);
    f = replaceStaffProfileTags(f);
    f = f.replace(/{AGE}/g, '');
    f = f.replace(/{AGE2}/g, '');
    f = f.replace(/{STAFFTYPE}/g, '');
    f = f.replace(/{ISCOUPLE}/g, '');
    f = f.replace(/\{.*\}/g, '0');
    
    return f;
}
function replacePercTags(f, subTotal, mpdGoal) {

    var temp = f.replace(/{SUBTOTAL}/g, subTotal);
    temp = temp.replace(/{MPDGOAL}/g, mpdGoal);
    return replaceTags(temp);
}



function setMinMax(m) {
    var min = $(m).attr('data-min');
    var max = $(m).attr('data-max');

    min = replaceTags(min);
    max = replaceTags(max);
    if (min == '') $(m).removeAttr('min');
    else $(m).attr('min', eval(min));



    if (max == '') $(m).removeAttr('max');
    else $(m).attr('max', eval(max));

}
