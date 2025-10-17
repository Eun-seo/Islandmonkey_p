package com.example.modutracker

import android.app.Activity
import android.content.res.Resources
import android.graphics.Bitmap
import android.graphics.Color
import android.graphics.drawable.BitmapDrawable
import android.graphics.drawable.Drawable
import android.util.Log
import androidx.core.content.ContextCompat
import com.prolificinteractive.materialcalendarview.CalendarDay
import com.prolificinteractive.materialcalendarview.DayViewDecorator
import com.prolificinteractive.materialcalendarview.DayViewFacade
import kotlin.math.pow
import kotlin.math.sqrt

//날짜 뒤에 이미지 삽입
class BackDecorator (context : Activity, currentDay: CalendarData) : DayViewDecorator {
    val color = listOf<ColorData>(ColorData("#FFEDAC","기분좋은"),
        ColorData("#BBF1C5","평범한"),
        ColorData("#AFE6E9","우울한"),
        ColorData("#FFCF83","뒤죽박죽한"),
        ColorData("#FFA4A4","화난"),
        ColorData("#D4CEFA","설레는") )

    private var drawable: Drawable = context.getDrawable(R.drawable.ic_baseline_favorite_24)!!
    private var myDay = currentDay.date
    private var colorIdx = currentDay.emotion.toString()
    val colorList = mutableListOf<String>()

    //이미지 캐시 저장용 맵
    companion object {
        private val imageCache = mutableMapOf<String, Drawable>()
    }

    override fun shouldDecorate(day: CalendarDay): Boolean {
        return day == myDay
    }

    override fun decorate(view: DayViewFacade) {
        setColor()

        //색상 조합 키 생성 (감정 4자리 색상코드 조합으로 구분)
        val key = colorList.joinToString("")

        //캐시된 이미지가 있으면 재사용, 없으면 생성
        val image = imageCache[key] ?: EmotionImage().also {
            imageCache[key] = it
            Log.d("BackDecorator", "새 이미지 생성: $key")
        }

        view.setBackgroundDrawable(image)
    }

    fun EmotionImage(): Drawable {
        val width = 256
        val height = 256
        val bitmap: Bitmap = Bitmap.createBitmap(width, height, Bitmap.Config.ARGB_8888)

        for (i in 0 until width) {
            for (j in 0 until height) {
                val margin = 40
                val imgWidth = (width - margin * 2).toDouble()
                val radius = imgWidth * sqrt(2.0) / (1 + sqrt(2.0)) / 2

                var color = 0

                val centers = listOf(
                    Pair(radius + margin, radius + margin),
                    Pair(imgWidth - radius + margin, radius + margin),
                    Pair(radius + margin, imgWidth - radius + margin),
                    Pair(imgWidth - radius + margin, imgWidth - radius + margin)
                )

                centers.forEachIndexed { idx, (cx, cy) ->
                    val dx = (cx - 1 - i)
                    val dy = (cy - 1 - j)
                    if (dx.pow(2) + dy.pow(2) <= radius.pow(2)) {
                        color = Color.parseColor(colorList[idx])
                    }
                }

                bitmap.setPixel(i, j, color)
            }
        }

        return BitmapDrawable(Resources.getSystem(), bitmap)
    }

    fun setColor() {
        colorList.clear()
        when(colorIdx.length) {
            4 ->{
                colorList.add(color[colorIdx[0].toString().toInt()-1].code)
                colorList.add(color[colorIdx[1].toString().toInt()-1].code)
                colorList.add(color[colorIdx[2].toString().toInt()-1].code)
                colorList.add(color[colorIdx[3].toString().toInt()-1].code)
            }
            3 ->{
                colorList.add(color[colorIdx[0].toString().toInt()-1].code)
                colorList.add(color[colorIdx[0].toString().toInt()-1].code)
                colorList.add(color[colorIdx[1].toString().toInt()-1].code)
                colorList.add(color[colorIdx[2].toString().toInt()-1].code)
            }
            2 ->{
                colorList.add(color[colorIdx[0].toString().toInt()-1].code)
                colorList.add(color[colorIdx[0].toString().toInt()-1].code)
                colorList.add(color[colorIdx[1].toString().toInt()-1].code)
                colorList.add(color[colorIdx[1].toString().toInt()-1].code)
            }
            1 ->{
                colorList.add(color[colorIdx[0].toString().toInt()-1].code)
                colorList.add(color[colorIdx[0].toString().toInt()-1].code)
                colorList.add(color[colorIdx[0].toString().toInt()-1].code)
                colorList.add(color[colorIdx[0].toString().toInt()-1].code)
            }
            else ->{
                colorList.add(color[colorIdx[0].toString().toInt()-1].code)
                colorList.add(color[colorIdx[1].toString().toInt()-1].code)
                colorList.add(color[colorIdx[2].toString().toInt()-1].code)
                colorList.add(color[colorIdx[3].toString().toInt()-1].code)
            }
        }
    }

    init {
        drawable = ContextCompat.getDrawable(context, R.drawable.ic_baseline_favorite_24)!!
    }
}
