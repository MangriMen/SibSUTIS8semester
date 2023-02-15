import re


def normalize_text(path: str):
    text = ""
    with open(path, 'r', encoding='utf-8') as file:
        text = ''.join(file.readlines())

    text = text.replace('\n', '')
    return re.sub(r'[^а-я\s]', '',
                  text.lower().translate(str.maketrans('ъё', 'ье')))
