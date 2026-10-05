---
title: Plan10K12Uker
layout: default
---

# Plan10K12Uker
En komplett 12‑ukers trenings- og kostholdsplan for høst/vinter.  
Mål: vektnedgang, økt VO2max, økt FTP, bedre metabolsk helse.

---

## Innhold

- [Blokk 1 – Uke 1–4](#blokk-1--uke-1-4)
- [Blokk 2 – Uke 5–8](#blokk-2--uke-5-8)
- [Blokk 3 – Uke 9–12](#blokk-3--uke-9-12)
- [Kostholdsplan](#kostholdsplan)
- [Treningskalender](#treningskalender)
- [JSON‑API](#json-api)

---

# Ukesstruktur

| Dag | Økt |
|-----|-----|
| Mandag | Løp intervall (4×4 eller VO2) |
| Tirsdag | Styrke (bein + rygg) |
| Onsdag | Zwift Z2 |
| Torsdag | Zwift VO2 eller FTP |
| Fredag | Styrke (bein + overkropp) |
| Lørdag | Løp Z2 |
| Søndag | Sykkel Z2 eller hvile |

---

# Blokk 1 – Uke 1–4

**Mål:** Bryte platå, øke VO2max, starte vektnedgang.

### Mandag – Løp 4×4
- 4 min @ 90–95 % makspuls  
- 3 min pause  

### Tirsdag – Styrke
- Knebøy 4×5  
- Markløft 3×5  
- Roing 4×8  
- Hip thrust 4×6  

### Onsdag – Zwift Z2
60–75 min

### Torsdag – Zwift VO2
6×2 min @ 120–130 % FTP

### Fredag – Styrke
Utfall, step-ups, militærpress, nedtrekk

### Lørdag – Løp Z2
5–10 km

### Søndag – Sykkel Z2
2–3 timer eller hvile

---

# Blokk 2 – Uke 5–8

**Mål:** Øke kapasitet, øke FTP, øke muskelmasse.

### Mandag – Løp 4×4 (progresjon)
Uke 5–6: 4×4  
Uke 7–8: 5×4  

### Tirsdag – Styrke (tyngre)
Knebøy 5×5, markløft 4×5

### Onsdag – Zwift Z2
60–90 min

### Torsdag – Zwift FTP
2×20 min eller 3×12 min

### Fredag – Styrke (volum)
Utfall, step-ups, press, nedtrekk

### Lørdag – Løp Z2
8–12 km

### Søndag – Sykkel Z2
Hvile eller rolig

---

# Blokk 3 – Uke 9–12

**Mål:** Peak vinterform, maksimal fettmobilisering.

### Mandag – Løp VO2
Uke 9–10: 10×1 min  
Uke 11–12: 6×3 min  

### Tirsdag – Styrke (vedlikehold)
Knebøy 3×5, markløft 2×5

### Onsdag – Zwift Z2
45–75 min

### Torsdag – Zwift VO2
8×2 min eller 5×3 min

### Fredag – Hvile eller lett styrke

### Lørdag – Løp Z2
10–14 km

### Søndag – Sykkel Z2
Rolig eller hvile

---

# Kostholdsplan

## Frokost
**Hviledager:**  
Proteinshake + peanøttsmør + bær  

**Treningsdager:**  
Proteinshake + peanøttsmør + banan/brødskive  

## Lunsj
Salat + egg + makrell + ekstra protein  

## Middag
Kylling, torsk, biff + grønnsaker + karbohydrater etter harde økter  

## Snacks
Sjokolade lørdager  
Moderate mengder alkohol  

---

# Treningskalender

| Uke | Mandag | Tirsdag | Onsdag | Torsdag | Fredag | Lørdag | Søndag |
|-----|--------|---------|--------|---------|--------|--------|--------|
| 1–4 | 4×4 | Styrke | Z2 | VO2 | Styrke | Løp Z2 | Sykkel Z2 |
| 5–6 | 4×4 prog | Styrke tung | Z2 | FTP | Styrke volum | Løp Z2 | Sykkel Z2 |
| 7–8 | 5×4 | Styrke tung | Z2 | FTP | Styrke volum | Løp Z2 | Sykkel Z2 |
| 9–10 | VO2 10×1 | Styrke vedl. | Z2 | VO2 | Hvile | Løp Z2 lang | Sykkel Z2 |
| 11–12 | VO2 6×3 | Styrke vedl. | Z2 | VO2 | Hvile | Løp Z2 lang | Sykkel Z2 |

---

# JSON‑API

JSON‑versjon av hele planen (for apper):

```json
{
  "name": "Plan10K12Uker",
  "blocks": [
    {
      "id": 1,
      "weeks": [1,2,3,4],
      "focus": "Bryte platå, øke VO2max",
      "schedule": {
        "Monday": "Run 4x4",
        "Tuesday": "Strength",
        "Wednesday": "Zwift Z2",
        "Thursday": "Zwift VO2",
        "Friday": "Strength",
        "Saturday": "Run Z2",
        "Sunday": "Bike Z2"
      }
    },
    {
      "id": 2,
      "weeks": [5,6,7,8],
      "focus": "Øke kapasitet, øke FTP",
      "schedule": {
        "Monday": "Run 4x4/5x4",
        "Tuesday": "Strength heavy",
        "Wednesday": "Zwift Z2",
        "Thursday": "Zwift FTP",
        "Friday": "Strength volume",
        "Saturday": "Run Z2",
        "Sunday": "Bike Z2"
      }
    },
    {
      "id": 3,
      "weeks": [9,10,11,12],
      "focus": "Peak vinterform",
      "schedule": {
        "Monday": "Run VO2",
        "Tuesday": "Strength maintenance",
        "Wednesday": "Zwift Z2",
        "Thursday": "Zwift VO2",
        "Friday": "Rest",
        "Saturday": "Run Z2 long",
        "Sunday": "Bike Z2"
      }
    }
  ]
}
