-> introduccion

=== introduccion ===
Estaba en un refugio cercano, pero me echaron porque había escasez de comida.

+ [¿Estuvo en contacto con otra gente?]
    -> contactoRefugio
+ [¿Por qué la echaron exactamente?]
    -> motivoEcharon
-> END


=== contactoRefugio ===
Solamente con la gente del refugio, estaban todos sanos.

+ [¿Presentó síntomas como picazón o sarpullido?]
    -> sintomas
+ [¿El refugio tenía controles médicos?]
    Sí, los hacían cada semana, nadie salió enfermo.
    -> END
-> END


=== sintomas ===
No, no, me siento perfectamente.

+ [¿Entonces qué son las manchas que tiene en la cara?]
    -> manchas
+ [Bien, si dice que está sana, confío en su palabra.]
    Gracias... no todos los guardias son tan comprensivos.
    -> END
-> END


=== manchas ===
Acné, lo tengo desde los 15 años, ya intenté de todo.

+ [No seré dermatólogo, pero esas ronchas no parecen acné…]
    ¿Qué podés saber vos entonces? Ya te dije que estoy sana, vengo de una zona segura.
    -> END
+ [Está bien, quizás me equivoqué.]
    Gracias... no todos los guardias son tan comprensivos.
    -> END
-> END


=== motivoEcharon ===
Había muy poca comida, empezaron a elegir quién se quedaba y quién no.

+ [¿Entonces convivió con otros hasta hace poco?]
    -> convivencia
+ [¿Cómo puedo confiar en su palabra?]
    Porque no tengo motivos para mentir. Solo quiero entrar.
    -> END
-> END


=== convivencia ===
Sí, pero estaban todos bien.

+ [¿Y esas manchas?]
    -> manchasMotivo
+ [Está bien, puede ser solo acné.]
    En serio, no estoy enferma.
    -> END
-> END


=== manchasMotivo ===
Acné, nada más.

+ [No estoy tan seguro… parecen infección.]
    ¿Y vos qué sabés? No soy ningún bicho raro.
    -> END
+ [Está bien, puede ser solo acné.]
    En serio, no estoy enferma.
    -> END
-> END


=== respuestaIngreso ===
¿En serio? Bueno... gracias.
-> END

=== respuestaRechazo ===
Ojalá te echen del laburo...
-> END
