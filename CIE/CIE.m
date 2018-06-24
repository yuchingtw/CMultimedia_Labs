load cie_1931.mat


figure(1)
plot(lambda, CIE_x, 'r');
hold on
plot(lambda, CIE_y, 'g');
hold on
plot(lambda, CIE_z, 'b');
xlabel('Wave length(\lambda)');
ylabel('Relative sensitivity');
title('CIE standrad XYZ color-matching functions');


x = CIE_x ./ (CIE_x + CIE_y + CIE_z);
y = CIE_y ./ (CIE_x + CIE_y + CIE_z);
z = 1 - x - y;
figure(2)
plot(x, 'r');
hold on;
plot(y, 'g');
hold on;
plot(z, 'b');
axis tight;
title('Normalize Data');


Nmat = [1.910 -0.532 -0.288; -0.985 2.0 -0.028; 0.058 -0.118 0.898];
invNmat = inv(Nmat);
R_xyz = invNmat * [1 0 0]';
Rx = R_xyz(1) / sum(R_xyz);
Ry = R_xyz(2) / sum(R_xyz);

G_xyz = invNmat * [0 1 0]';
Gx = G_xyz(1) / sum(G_xyz);
Gy = G_xyz(2) / sum(G_xyz);

B_xyz = invNmat * [0 0 1]';
Bx = B_xyz(1) / sum(B_xyz);
By = B_xyz(2) / sum(B_xyz);

a = [Rx Gx Bx Rx];
b = [Ry Gy By Ry];
x = [x; x(1)];
y = [y; y(1)];

figure(3);
plot(x, y, a, b);
grid;
xlabel('x');
ylabel('y');
title('CIE chromaticity diaram');