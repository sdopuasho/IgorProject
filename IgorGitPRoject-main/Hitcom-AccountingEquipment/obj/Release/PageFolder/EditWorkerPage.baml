   M S B A M L   `   `   `  ���� U  QHitcom-AccountingEquipment, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]3clr-namespace:Hitcom_AccountingEquipment.PageFolder%Hitcom_AccountingEquipment.PageFolder  G(clr-namespace:Hitcom_AccountingEquipmentHitcom_AccountingEquipment  :    4Hitcom_AccountingEquipment.PageFolder.EditWorkerPage   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     Elocal3clr-namespace:Hitcom_AccountingEquipment.PageFolder     <local21(clr-namespace:Hitcom_AccountingEquipment       N� Title$  EditWorkerPage��.���  � ColumnDefinitions �� $	��200 ��� $��* � � RowDefinitions �� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $	��150 ��� $��50 �.+��� $��Фамилия��$��0�����#a961a0$��Moloto)�$��20(�$��10q�$��Center=�$��Center=��� $��Имя��$��1�����#a961a0$��Moloto)�$��20(�$��10q�$��Center=�$��Center=��� $��Отчество��$��2�����#a961a0$��Moloto)�$��20(�$��10q�$��Center=�$��Center=��� $��Пол��$��3�����#a961a0$��Moloto)�$��20(�$��10q�$��Center=�$��Center=��� $��Телефон��$��4�����#a961a0$��Moloto)�$��20(�$��10q�$��Center=�$��Center=��� $��
Почта��$��5�����#a961a0$��Moloto)�$��20(�$��10q�$��Center=�$��Center=��� $��Должность��$��6�����#a961a0$��Moloto)�$��20(�$��10q�$��Center=�$��Center=��� $��Статус��$��7�����#a961a0$��Moloto)�$��20(�$��10q�$��Center=�$��Center=��� $��Фотография��$��8�����#a961a0$��Moloto)�$��20(�$��10q�$��Center=�$��Center=��� $��0��$��1�� �� Orientation$ Vertical=�.+���  �� CornerRadius$ 10�����#f18e57$��1q�$��0��$��1��$
��Left=�$��Center=� � UseLayoutRounding . $��35��$	��260��$��5q�._��� -    ��Name$ FName��$��0��$��1��$��5q����#a961a0$��Moloto)�$��20(�$��Center=�$��Center=�$��0q�
������ $	��250��$��25������ *	FirstName+.���� -    ��Name$ 	FnameFail��
���  ��$y�	Collapsed=��� $��1��$��1��$ Vertical=�.+��� $ 10�����#f18e57$��1q�$��1��$��1��$
��Left=�$��Center=� . $��35��$	��260��$��5q�._��� -   $ LName��$��1��$��1��$��5q�$��Center=�$��Center=����#a961a0$��Moloto)�$��20(�$��0q�
������ $	��250��$��25������ *
LastName+�� -   $ 	LnameFail��
���  ��$y�	Collapsed=��� $��2��$��1��$ Vertical=�.+��� $ 10�����#f18e57$��1q�$��2��$��1��$
��Left=�$��Center=� . $��35��$	��260��$��5q�._��� -   $ MName��$��2��$��1��$��Center=�$��Center=����#a961a0$��5q�$��Moloto)�$��20(�$��0q�
������ $	��250��$��25������ *
MiddleName+�� -   $ 	MnameFail��
���  ��$y�	Collapsed=��� $��3��$��1��$ Vertical=�.+��� $ 10�����#f18e57$��1q�$��1��$��3��$
��Left=�$��Center=� . $��35��$	��260��$��5q�._��� -    ��Name$ ComboGender��$��1��$��3��$��Center=�$��Center=�
����������#a961a0$��0q�$��Moloto)�$��20(�$	��250��$��25��$��5q�	 �� DisplayMemberPath$	 
GenderName��
 �� SelectedItem
 �� *Gender+ � 	Resources  �� 
TargetType �� Property �� Value��%�   '��       �� " ��.���  � �� �#a961a0��  � ��$ Moloto)���  � ��$ 20(���  � ��$ 35����  � �� �	#FF232D3E�� -   $ 
GenderFail��
���  ��$y�	Collapsed=��� $��4��$��1��$ Vertical=�.+��� $ 10�����#f18e57$��1q�$��4��$��1��$
��Left=�$��Center=� . $��35��$	��260��$��5q�._��� -	   $ Phone��$��4��$��1��$��5q�$��Center=�$��Center=����#a961a0$��Moloto)�$��20(�$��0q�
������ $	��250��$��25������ *PhoneNumber+�� -
   $ 	PhoneFail��
���  ��$y�	Collapsed=��� $��5��$��1��$ Vertical=�.+��� $ 10�����#f18e57$��1q�$��5��$��1��$
��Left=�$��Center=� . $��35��$	��260��$��5q�._��� -   $ EmailTxt��$��5��$��1��$��Center=�$��Center=�$��5q����#a961a0$��Moloto)�$��20(�$��0q�
������ $	��250��$��25������ *EmailOfWorker+�� -   $ 	EmailFail��
���  ��$y�	Collapsed=��� $ 10�����#f18e57$��1q�$��8��$��1��$
��Left=�$��Center=� . $��5q�._��� $ 
Horizontal=�$��8��$��1��.+��� -    ��Name$ PhotoWorker��$	��130��$	��150��$��5q��� -    ��Name$ DialogToPath�����#e5266f$��0q�$��Moloto)�$��20(�$
��Left=�
������ $��Center=�$��5q�$	��200��$'��!Выбор изображения���� $��6��$��1��$ Vertical=�.+��� $ 10�����#f18e57$��1q�$��1��$��6��$
��Left=�$��Center=� . $��35��$	��260��$��5q�._��� -   $ ComboPosition��$��1��$��7��$��5q�$��Center=�$��Center=�
����������#a961a0$��0q�$��Moloto)�$��20(�$	��250��$��25��$	 PostionName��
 �� *
Position+ ��%�   '��       �� " ��.���  � �� �#a961a0��  � ��$ Moloto)���  � ��$ 20(���  � ��$ 35����  � �� �	#FF232D3E�� -   $ PositionFail��
���  ��$y�	Collapsed=��� $��7��$��1��$ Vertical=�.+��� $ 10�����#f18e57$��1q�$��1��$��7��$
��Left=�$��Center=� . $��35��$	��260��$��5q�._��� -   $ ComboStatus��$��1��$��7��$��5q�$��Center=�$��Center=�
����������#a961a0$��0q�$��Moloto)�$��20(�$	��250��$��25��$	 NameOfStatus��
 �� *StatusOfWorker+ ��%�   '��       �� " ��.���  � �� �#a961a0��  � ��$ Moloto)���  � ��$ 20(���  � ��$ 35����  � �� �	#FF232D3E�� -   $ StatusWorkerFail��
���  ��$y�	Collapsed=��� -   $��BtnSave��$��Сохранить��$��9��$��1�����#e5266f$��0q�$��Moloto)�$��20(�$
��Left=�
������ $��Bottom=�$��10,0,0,8q�$	��175��$��34��