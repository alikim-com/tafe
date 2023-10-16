The idea behind Task Five is to build a scalable chart instead of displaying one symbol per unit of sales because that will overflow the console viewport if the numbers are large enough.

So, on one hand, there is a range of sales and on the other hand there is a bar chart range defined by (configurable) min and max length of a bar.

To map the first range to the second, a universal unit [0, 1] range is used as an intermediary.

The unit range value of sales is calculated as v<sub>u</sub> = (v<sub>sales</sub> - min<sub>sales</sub>) / (max<sub>sales</sub> - min<sub>sales</sub>), promoted to **double**.

The size of a bar is calculated as v<sub>chart</sub> = min<sub>chart</sub> + v<sub>u</sub> * (max<sub>chart</sub> - min<sub>chart</sub>), rounded to the nearest **double** and then demoted to **int**.
