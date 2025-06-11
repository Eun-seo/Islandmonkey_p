"use strict";

const mysql = require("../config/mysql");
const request = require("request");
const jwt = require("jsonwebtoken");

const { verifyToken } = require('../routes/middlewares');

let Diary = function (diary, token) {
    this.groupidx = diary.groupidx;
    this.useridx = diary.useridx;
    this.content = diary.content;
    this.emotionidx = diary.emotionidx;
    this.date = diary.date;
    this.token = token;
};

//일기 입력
Diary.addDiary = function (newEmp, result){
     const data =  jwt.verify(newEmp.token, 'modu_key');
     const userid = data.idCode;

    mysql.query('Insert into diary(useridx, content, emotionidx) ' +
        'select (select useridx from user where id = ? limit 1),?,? from dual ' +
        'where not exists (select useridx from diary where useridx = (select useridx from user where id = ? limit 1) and date(date) = date(now()))',
        [userid, newEmp.content, newEmp.emotionidx, userid], function (err, res){
        if(err){
            console.log("error: ", err);
            result(null, res);
        }
        else{
            console.log(res.insertId);
            result(null, res.insertId);
        }
    });
};

//일기 조회
Diary.inquire = function (newEmp,result) {

    const data =  jwt.verify(newEmp.token, 'modu_key');
    const userid = data.idCode;

    mysql.query('Select u.name, d.emotionidx, d.content from user as u join diary as d on u.useridx = d.useridx && u.groupidx = (select groupidx from user where id = ?) where date(date) = date(?) order by date desc',
        [userid,newEmp.date],
        function (err, res) {
        if(err) {
            console.log("error: ", err);
            result(null, res);
        }
        else {
            console.log("result : ", res);
            result(null, res);
        }
    });
};

//무드트래커 조회
Diary.calendar = function (token, result) {

    const data =  jwt.verify(token, 'modu_key');
    const userid = data.idCode;

    mysql.query('Select d.emotionidx, DATE(d.date) as datedata ' +
        'from diary as d ' +
        '    inner join user as u on u.useridx = d.useridx ' +
        '    inner join group_table gt on u.groupidx = gt.groupidx ' +
        'where gt.groupidx = (select groupidx from user where id = ?)',
        [userid],
        function (err, res) {
            if(err) {
                console.log("error: ", err);
                result(null, res);
            }
            else {
                console.log("result : ", res);
                result(null, res);
            }
        });
};

//오늘 일기 썼는지 확인
Diary.checkDiary = function (token,result) {

    const data =  jwt.verify(token, 'modu_key');
    const userid = data.idCode;

    mysql.query('select exists (select * from diary where useridx = (select useridx from user where id = ?) and DATE(date) = DATE(now())) as result',
        [userid],
        function (err, res) {
            if(err) {
                console.log("error: ", err);
                result(null, res);
            }
            else {
                console.log("result : ", res);
                result(null, res);
            }
        });
};

module.exports = Diary;
