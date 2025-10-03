-> introduccion

=== introduccion ===
Hola guardia... llevo horas esperando.
+ [¿Estuvo en contacto con otras personas?]
    -> contactoPersonas
+ [¿Qué pasó en el camino?]
    -> lluviaCamino

=== contactoPersonas ===
No, no me crucé con nadie en el camino.
+ [¿Seguro que nadie más estuvo cerca?]
    —Sí, estoy segura, no vi a nadie.
    -> END
+ [¿Se siente bien de salud?]
    —Sí, solo estoy cansada y hambrienta.
    -> END

=== lluviaCamino ===
Me agarró la lluvia y me perdí entre los caminos de tierra.
+ [Vaya, eso suena agotador.]
    —Sí, solo quiero un lugar para limpiar y descansar.
    -> END
+ [No puedo ayudarle ahora.]
    —De acuerdo, seguiré caminando.
    -> END

=== respuestaIngreso ===
¡Gracias! Por fin voy a poder sacarme esta tierra.
-> END

=== respuestaRechazo ===
Entiendo... voy a seguir caminando...
-> END
