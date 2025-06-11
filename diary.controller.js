"use strict";

const Diary = require("../model/diary");


exports.addDiary = function (req, res){
    const token = req.headers.authorization;
    const new_diary = new Diary(req.body, token);

    if(req.body.constructor === Object && Object.keys(req.body).length === 0) {
        res
            .status(400)
            .send({error : true, message: "Please provide all required field"});
    } else {
        Diary.addDiary(new_diary, function (err, diary) {
            if (err) res.send(err);
            res.json({
                data: diary,
            });
        });
    }
};

exports.inquire = function (req, res) {

    const token = req.headers.authorization;
    const new_diary = new Diary(req.body, token);

    Diary.inquire(new_diary,function (err, diary){
        console.log("controller");
        if(err) res.send(err);
        console.log("res", diary);
        res.json({
            data: diary,
        });
    });
};

exports.calendar = function (req, res) {

    const token = req.headers.authorization;

    Diary.calendar(token,function (err, cal){
        console.log("controller");
        if(err) res.send(err);
        console.log("res", cal);
        res.json({
            data: cal,
        });
    });
};

exports.checkDiary = function (req, res) {

    const token = req.headers.authorization;

    Diary.checkDiary(token,function (err, result){
        console.log("controller");
        if(err) res.send(err);
        console.log("res", result);
        res.json({
            data: result,
        });
    });
};
