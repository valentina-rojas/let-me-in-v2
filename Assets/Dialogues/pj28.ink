-> introduccion

=== introduccion ===
Hola… traje los globos, pero se me escaparon todos. La fiesta se fue sin mí.

+ [¿Por qué viene disfrazado de payaso?]
    No recuerdo bien… creo que era para hacer reír, pero ya nadie escucha.
    -> destino
+ [¿Sabe a dónde va?]
    No… solo sé que no quiero estar afuera. La calle está demasiado vacía… y ruidosa a la vez.
    -> sintomas
-> END


=== destino ===
+ [Está bien, pero necesito saber si tiene síntomas.]
    Tos… un poco. Pero más que nada, tengo frío… y ganas de que alguien me cuente un chiste.
    -> sintomas
+ [¿Está seguro de querer entrar así?]
    Sí… necesito un lugar donde no sentirme tan solo.
    -> sintomas
-> END


=== sintomas ===
+ [Tiene tos, ¿está enfermo?]
    Solo un poquito… prometo no molestar a nadie.
    -> END
+ [Debe cuidar su salud y la de otros.]
    Sí… por eso busco un lugar seguro.
    -> END
-> END


=== respuestaIngreso ===
Gracias, espero que alguien me traiga algo de alegría.
-> END

=== respuestaRechazo ===
Adiós.
-> END
